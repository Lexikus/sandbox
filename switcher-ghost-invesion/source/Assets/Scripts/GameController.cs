using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

using InputKey = CharacterInput.InputKey;

using Random = UnityEngine.Random;

public class GameController : MonoBehaviour {

    public static GameController Instance { get; private set; }

    private EnemySpawnpointManager enemySpawnpointManager;
    private PlayerSpawnpointManager playerSpawnpointManager;

    [SerializeField] protected List<GameObject> enemiesPrefab = new List<GameObject>();
    [SerializeField] protected GameObject playerPrefab;
    [SerializeField] protected GameObject daggerPrefab;

    public Dictionary<int, GameObject> PlayerDummies { get; protected set; }
    public Dictionary<int, GameObject> EnemyDummies { get; protected set; }
    public Dictionary<int, GameObject> DaggerDummies { get; protected set; }

    private int enemyIndexer;
    private int daggerIndexer;

    private const int MAX_ENEMIES = 100;

    private void Awake() {
        PlayerDummies = new Dictionary<int, GameObject>();
        EnemyDummies = new Dictionary<int, GameObject>();
        DaggerDummies = new Dictionary<int, GameObject>();

        if (Instance == null) {
            Instance = this;
        }
        else if (Instance != this) Destroy(this);
    }

    protected virtual void Start() {
        enemySpawnpointManager = EnemySpawnpointManager.Instance;
        playerSpawnpointManager = PlayerSpawnpointManager.Instance;
    }

    protected virtual Transform GetPlayerTransform() {
        return PlayerDummies.ElementAt(Random.Range(0, PlayerDummies.Count())).Value.transform;
    }

    public virtual GameObject SpawnEnemy(int id) {
        GameObject prefab = GetRandomEnemyPrefab();
        Vector2 spawnPosition = enemySpawnpointManager.GetRandomSpawnpoint();

        GameObject enemyGameObject = Instantiate(prefab, spawnPosition, Quaternion.identity);
        EnemyDummy enemyDummy = enemyGameObject.AddComponent<EnemyDummy>();
        enemyDummy.NewPosition = spawnPosition;

        EnemyDummies.Add(id, enemyGameObject);

        return enemyGameObject;
    }

    public virtual GameObject SpawnDagger(int id, Vector2 position, Quaternion rotation) {
        if (playerPrefab == null) return null;

        GameObject daggerGameObject = Instantiate(daggerPrefab, position, rotation);
        DaggerDummy daggerDummy = daggerGameObject.AddComponent<DaggerDummy>();
        daggerDummy.NewPosition = position;
        daggerDummy.NewRotation = rotation;

        DaggerDummies.Add(id, daggerGameObject);

        return daggerGameObject;
    }

    public GameObject SpawnDagger(Vector2 position, Quaternion rotation) {
        return SpawnDagger(++daggerIndexer, position, rotation);
    }

    public void DespawnDagger(int id) {
        if (!DaggerDummies.ContainsKey(id)) return;

        GameObject daggerDummy = DaggerDummies[id];
        DaggerDummies.Remove(id);
        Destroy(daggerDummy);
    }

    public virtual GameObject SpawnPlayer(int id) {
        if (playerPrefab == null) return null;

        Vector2 spawnPosition = playerSpawnpointManager.GetSpawnpoint(id);
        GameObject playerGameObject = Instantiate(playerPrefab, spawnPosition, Quaternion.identity);
        PlayerDummy playerDummy = playerGameObject.AddComponent<PlayerDummy>();
        playerDummy.NewPosition = spawnPosition;

        PlayerDummies.Add(id, playerGameObject);

        return playerGameObject;
    }

    public GameObject SpawnPlayer() {
        return SpawnPlayer(PlayerDummies.Count + 1);
    }

    public void SpawnEnemies() {
        if (EnemyDummies.Count >= MAX_ENEMIES) return;
        SpawnEnemy(++enemyIndexer);
    }

    public virtual void DespawnEnemy(int id) {
        if (!EnemyDummies.ContainsKey(id)) return;

        GameObject enemyDummy = EnemyDummies[id];
        EnemyDummies.Remove(id);
        Destroy(enemyDummy, 1f);
    }

    public virtual void DespawnPlayer(int id) {
        if (!PlayerDummies.ContainsKey(id)) return;
        GameObject player = PlayerDummies[id];
        PlayerDummies.Remove(id);
        Destroy(player);
    }

    private GameObject GetRandomEnemyPrefab() {
        if (enemiesPrefab.Count == 0) return null;
        return enemiesPrefab[Random.Range(0, enemiesPrefab.Count)];
    }

    public virtual void OnInputPress(InputKey inputKey) { }

    public virtual void OnInputMovement(Vector2 movement, InputKey inputKey) {
        if (PlayerDummies.Count <= 0) return;

        movement = movement.normalized;
        PlayerDummies.ElementAt(0).Value.transform.Translate(movement * 5 * Time.deltaTime);
    }

    protected void SetTimeout(Action callback, float timer) {
        StartCoroutine(IESetTimeout(callback, timer));
    }

    private IEnumerator IESetTimeout(Action callback, float timer) {
        yield return new WaitForSeconds(timer);
        callback();
    }
}
