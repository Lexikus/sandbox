using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using InputKey = CharacterInput.InputKey;

public class NetworkGameController : GameController {

    public Network Network { get; private set; }

    private NetworkDataHandler playerIdentifierDataHandler;
    private NetworkDataHandler playerConnectionsHandler;
    private NetworkDataHandler playerDisconnectedHandler;
    private NetworkDataHandler playerUpdatePositionDataHandler;
    private NetworkDataHandler serverPlayerMovementInputDataHandler;
    private NetworkDataHandler serverPlayerShootInputDataHandler;
    private NetworkDataHandler serverPlayerReadyDataHandler;
    private NetworkDataHandler enemyInformationsDataHandler;
    private NetworkDataHandler enemyUpdatePositionDataHandler;
    private NetworkDataHandler playerDiedDataHandler;
    private NetworkDataHandler enemyDiedDataHandler;
    private NetworkDataHandler playerHealthChangedDataHandler;
    private NetworkDataHandler daggerInformationsDataHandler;
    private NetworkDataHandler daggerBrokeDataHandler;
    private NetworkDataHandler daggerUpdatePositionDataHandler;

    public Dictionary<int, Player> Players { get; private set; }
    public Dictionary<int, Enemy> Enemies { get; private set; }

    [SerializeField] private float spawnIntervalTimer = 2;
    [SerializeField] private float spawnOffsetTimer = 3;

    private Vector2 lastMovement = Vector2.zero;

    protected override void Start() {
        base.Start();

        Players = new Dictionary<int, Player>();
        Enemies = new Dictionary<int, Enemy>();

        Network = Network.Instance;
        if (Network == null) {
            return;
        }

        Network.DataEvent.AddListener(OnData);

        playerIdentifierDataHandler = new NetworkPlayerIdentifierDataHandler(this);
        playerConnectionsHandler = new NetworkPlayerConnectionDataHandler(this);
        playerDisconnectedHandler = new NetworkPlayerDisconnectedDataHandler(this);
        playerUpdatePositionDataHandler = new NetworkPlayerUpdatePositionDataHandler(this);
        serverPlayerMovementInputDataHandler = new NetworkServerPlayerMovementInputDataHandler(this);
        serverPlayerShootInputDataHandler = new NetworkServerPlayerShootInputDataHandler(this);
        serverPlayerReadyDataHandler = new NetworkServerPlayerReadyDataHandler(this);
        enemyInformationsDataHandler = new NetworkEnemyInformationsDataHandler(this);
        enemyUpdatePositionDataHandler = new NetworkEnemyUpdatePositionDataHandler(this);
        playerDiedDataHandler = new NetworkPlayerDiedDataHandler(this);
        enemyDiedDataHandler = new NetworkEnemyDiedDataHandler(this);
        playerHealthChangedDataHandler = new NetworkPlayerHealthChangedDataHandler(this);
        daggerInformationsDataHandler = new NetworkDaggerInformationsDataHandler(this);
        daggerBrokeDataHandler = new NetworkDaggerBrokeDataHandler(this);
        daggerUpdatePositionDataHandler = new NetworkDaggerUpdatePositionDataHandler(this);

        if (!Network.IsServer) {
            RequestConnections();
            return;
        }

        DeathCamera.Instance.Enable();

        Network.ConnectEvent.AddListener(OnConnect);
        Network.DisconnectEvent.AddListener(OnDisconnect);

        InvokeRepeating("SendPlayerPositions", 0, Network.GAME_TICK);
        InvokeRepeating("SendEnemyPositions", 0, Network.GAME_TICK);
        InvokeRepeating("SendDaggerPositions", 0, Network.GAME_TICK);
        InvokeRepeating("SpawnEnemies", spawnOffsetTimer, spawnIntervalTimer);
    }

    private void SendPlayerPositions() {
        if (PlayerDummies.Count == 0) return;

        string message = MessageHeaders.PLAYER_UPDATE_POSITION + "|";
        foreach (var player in Players) {
            Vector2 position = (Vector2)player.Value.transform.position;
            int lastInputKey = (int)player.Value.LastMovementInputKey;
            message += player.Key + "%" + position.x + "%" + position.y + "%" + lastInputKey + "|";
        }
        message = message.Trim('|');
        Network.Send(message, Network.UnreliableChannelId);
    }

    private void SendEnemyPositions() {
        if (EnemyDummies.Count == 0) return;

        string message = MessageHeaders.ENEMY_UPDATE_POSITION + "|";
        foreach (var enemy in EnemyDummies) {
            Vector2 position = (Vector2)enemy.Value.transform.position;
            string enemyInfo = enemy.Key + "%" + position.x + "%" + position.y + "|";
            // if there is not enough space for the message, we have to split it in multiple messages.
            if ((message.Length + enemyInfo.Length) * sizeof(char) < Network.BUFFER_SIZE) {
                message += enemyInfo;
            }
            else {
                message = message.Trim('|');
                Network.Send(message, Network.UnreliableChannelId);
                message = MessageHeaders.ENEMY_UPDATE_POSITION + "|";
            }
        }
        message = message.Trim('|');
        Network.Send(message, Network.UnreliableChannelId);
    }

    private void SendDaggerPositions() {
        string message = MessageHeaders.DAGGER_UPDATE_POSITION + "|";
        foreach (var _dagger in DaggerDummies) {
            string daggerInfo = _dagger.Key + "%";
            Vector2 position = _dagger.Value.transform.position;
            Quaternion rotation = _dagger.Value.transform.rotation;

            daggerInfo += position.x + "%";
            daggerInfo += position.y + "%";
            daggerInfo += rotation.eulerAngles.z + "|";

            if ((message.Length + daggerInfo.Length) * sizeof(char) < Network.BUFFER_SIZE) {
                message += daggerInfo;
            }
            else {
                message = message.Trim('|');
                Network.Send(message, Network.ReliableChannelId);
                message = MessageHeaders.DAGGER_INFORMATIONS + "|";
            }
        }
        message = message.Trim('|');
        Network.Send(message, Network.ReliableChannelId);
    }

    private void OnData(string message) {
        if (message.Length <= 0) return;

        string[] data = message.Split(new char[] { '|' }, 2);

        if (data.Length <= 1) return;

        switch (data[0]) {
            case MessageHeaders.PLAYER_IDENTIFIER: playerIdentifierDataHandler.HandleMessage(data[1]); break;
            case MessageHeaders.PLAYER_CONNECTIONS: playerConnectionsHandler.HandleMessage(data[1]); break;
            case MessageHeaders.PLAYER_DISCCONNECTED: playerDisconnectedHandler.HandleMessage(data[1]); break;
            case MessageHeaders.PLAYER_UPDATE_POSITION: playerUpdatePositionDataHandler.HandleMessage(data[1]); break;
            case MessageHeaders.SERVER_PLAYER_MOVEMENT_INPUT: serverPlayerMovementInputDataHandler.HandleMessage(data[1]); break;
            case MessageHeaders.SERVER_PLAYER_READY: serverPlayerReadyDataHandler.HandleMessage(data[1]); break;
            case MessageHeaders.ENEMY_INFORMATIONS: enemyInformationsDataHandler.HandleMessage(data[1]); break;
            case MessageHeaders.ENEMY_UPDATE_POSITION: enemyUpdatePositionDataHandler.HandleMessage(data[1]); break;
            case MessageHeaders.PLAYER_DIED: playerDiedDataHandler.HandleMessage(data[1]); break;
            case MessageHeaders.ENEMY_DIED: enemyDiedDataHandler.HandleMessage(data[1]); break;
            case MessageHeaders.PLAYER_HEALTH_CHANGED: playerHealthChangedDataHandler.HandleMessage(data[1]); break;
            case MessageHeaders.SERVER_PLAYER_SHOOT_INPUT: serverPlayerShootInputDataHandler.HandleMessage(data[1]); break;
            case MessageHeaders.DAGGER_INFORMATIONS: daggerInformationsDataHandler.HandleMessage(data[1]); break;
            case MessageHeaders.DAGGER_BROKE: daggerBrokeDataHandler.HandleMessage(data[1]); break;
            case MessageHeaders.DAGGER_UPDATE_POSITION: daggerUpdatePositionDataHandler.HandleMessage(data[1]); break;
            default: Debug.Log("Unknown message type: " + data[0]); break;
        }
    }

    private void OnConnect(int id) {
        SpawnPlayer(id);
    }

    private void RequestConnections() {
        string message = MessageHeaders.SERVER_PLAYER_READY + "|";
        Network.Send(message, Network.ReliableChannelId);
    }

    private void OnDisconnect(int id) {
        DespawnPlayer(id);

        string message = MessageHeaders.PLAYER_DISCCONNECTED + "|" + id;
        Network.Send(message, Network.ReliableChannelId);
    }

    public override GameObject SpawnPlayer(int id) {
        GameObject playerGameObject = base.SpawnPlayer(id);

        if (Network.IsServer) {
            NetworkEntity entity = Network.Connections.Find(x => x.ID == id);
            if (entity == null) return null;
            if (entity.Dead) return null;

            Destroy(playerGameObject.GetComponent<PlayerDummy>());
            Player player = playerGameObject.AddComponent<Player>();
            player.Id = id;
            player.OnDying = OnPlayerDying;
            player.OnHealthChange = OnPlayerHealthChange;
            Players.Add(id, player);

            playerGameObject.AddComponent<BoxCollider2D>();

            Rigidbody2D rigidbody2D = playerGameObject.AddComponent<Rigidbody2D>();
            rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
            rigidbody2D.gravityScale = 0;
        }

        return playerGameObject;
    }

    private void OnPlayerDying(int id) {
        DespawnPlayer(id);

        NetworkEntity entity = Network.Connections.Find(x => x.ID == id);
        if (entity != null) entity.Dead = true;

        string message = MessageHeaders.PLAYER_DIED + "|" + id;
        Network.Send(message, Network.ReliableChannelId);
    }

    private void OnPlayerHealthChange(int id, int health) {
        string message = MessageHeaders.PLAYER_HEALTH_CHANGED + "|" + id + "|" + health;
        Network.Send(message, Network.ReliableChannelId);
    }

    private void OnEnemyDying(int id) {
        DespawnEnemy(id);

        string message = MessageHeaders.ENEMY_DIED + "|" + id;
        Network.Send(message, Network.ReliableChannelId);
    }

    public override void DespawnPlayer(int id) {
        base.DespawnPlayer(id);

        if (Network.IsServer) Players.Remove(id);
    }

    public override GameObject SpawnEnemy(int id) {
        if (Network.IsServer) {
            var alivePlayers = Players.Where(x => !x.Value.IsDead);
            if (alivePlayers.Count() == 0) return null;

            Transform playerTransform = GetPlayerTransform();
            if (playerTransform == null) return null;

            GameObject enemyGameObject = base.SpawnEnemy(id);
            Destroy(enemyGameObject.GetComponent<EnemyDummy>());
            Enemy enemy = enemyGameObject.AddComponent<Enemy>();
            enemy.Id = id;
            enemy.SetTarget(playerTransform);
            enemy.OnDying = OnEnemyDying;

            enemyGameObject.AddComponent<BoxCollider2D>();
            Rigidbody2D rigidbody2D = enemyGameObject.AddComponent<Rigidbody2D>();
            rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
            rigidbody2D.gravityScale = 0;

            Enemies.Add(id, enemy);

            string message = MessageHeaders.ENEMY_INFORMATIONS + "|";
            foreach (var _enemy in EnemyDummies) {
                message += _enemy.Key + "|";
            }
            message = message.Trim('|');
            Network.Send(message, Network.ReliableChannelId);

            return enemyGameObject;
        }
        else
            return base.SpawnEnemy(id);
    }

    public override void DespawnEnemy(int id) {
        base.DespawnEnemy(id);

        if (Network.IsServer) Enemies.Remove(id);
    }

    public override GameObject SpawnDagger(int id, Vector2 position, Quaternion rotation) {
        GameObject daggerGameObject = base.SpawnDagger(id, position, rotation);

        if (Network.IsServer) {
            Destroy(daggerGameObject.GetComponent<DaggerDummy>());
            Dagger dagger = daggerGameObject.AddComponent<Dagger>();
            dagger.Id = id;
            dagger.OnBreak = OnDaggerBreak;

            BoxCollider2D boxCollider2D = daggerGameObject.AddComponent<BoxCollider2D>();
            boxCollider2D.offset = new Vector2(-0.05f, -0.05f);
            boxCollider2D.size = new Vector2(0.35f, 0.7f);

            Rigidbody2D rigidbody2D = daggerGameObject.AddComponent<Rigidbody2D>();
            rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
            rigidbody2D.gravityScale = 0;

            string message = MessageHeaders.DAGGER_INFORMATIONS + "|";
            foreach (var _dagger in DaggerDummies) {
                string daggerInfo = _dagger.Key + "%";
                daggerInfo += position.x + "%";
                daggerInfo += position.y + "%";
                daggerInfo += rotation.eulerAngles.z + "|";

                if ((message.Length + daggerInfo.Length) * sizeof(char) < Network.BUFFER_SIZE) {
                    message += daggerInfo;
                }
                else {
                    message = message.Trim('|');
                    Network.Send(message, Network.ReliableChannelId);
                    message = MessageHeaders.DAGGER_INFORMATIONS + "|";
                }
            }
            message = message.Trim('|');
            Network.Send(message, Network.ReliableChannelId);
        }

        return daggerGameObject;
    }

    private void OnDaggerBreak(int id) {
        DespawnDagger(id);

        string message = MessageHeaders.DAGGER_BROKE + "|" + id;
        Network.Send(message, Network.ReliableChannelId);
    }

    protected override Transform GetPlayerTransform() {
        var alivePlayers = Players.Where(x => !x.Value.IsDead);

        if (alivePlayers.Count() == 0) return null;

        return alivePlayers.ElementAt(Random.Range(0, alivePlayers.Count())).Value.transform;
    }

    public override void OnInputPress(InputKey inputKey) {
        if (Network.IsServer) return;
        Client client = Network as Client;
        if (client.Id == 0) return;

        string message = MessageHeaders.SERVER_PLAYER_SHOOT_INPUT + "|" + client.Id + "|";
        message += (byte)inputKey;
        Network.Send(message, Network.ReliableChannelId);
    }

    public override void OnInputMovement(Vector2 movement, InputKey inputKey) {
        if (Network.IsServer) return;
        Client client = Network as Client;
        if (client.Id == 0) return;

        if (movement == lastMovement) return;

        lastMovement = movement;

        string message = MessageHeaders.SERVER_PLAYER_MOVEMENT_INPUT + "|" + client.Id + "|";
        message += movement.x + "%" + movement.y + "%" + (int)inputKey;
        Debug.Log(message);
        Network.Send(message, Network.ReliableChannelId);
    }
}