using UnityEngine;

public class NetworkServerPlayerReadyDataHandler : NetworkDataHandler {
    public NetworkServerPlayerReadyDataHandler(NetworkGameController gameController) : base(gameController) { }
    public override void HandleMessage(string ignore) {

        // tell all players about the other players
        string message = MessageHeaders.PLAYER_CONNECTIONS + "|";
        foreach (var entity in gameController.Network.Connections) {
            if (entity.Dead) continue;
            message += entity.ID + "|";
        }
        message = message.Trim('|');

        gameController.Network.Send(message, gameController.Network.ReliableChannelId);

        // tell all players about the enemies
        message = MessageHeaders.ENEMY_INFORMATIONS + "|";
        foreach (var enemy in gameController.EnemyDummies) {
            message += enemy.Key + "|";
        }
        message = message.Trim('|');
        gameController.Network.Send(message, gameController.Network.ReliableChannelId);

        // tell all players about the daggers
        message = MessageHeaders.DAGGER_INFORMATIONS + "|";
        foreach (var _dagger in gameController.DaggerDummies) {
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
                gameController.Network.Send(message, gameController.Network.ReliableChannelId);
                message = MessageHeaders.DAGGER_INFORMATIONS + "|";
            }
        }
        message = message.Trim('|');
        gameController.Network.Send(message, gameController.Network.ReliableChannelId);
    }
}