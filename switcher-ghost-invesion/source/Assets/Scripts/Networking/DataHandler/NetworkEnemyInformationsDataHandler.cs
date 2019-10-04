using UnityEngine;

public class NetworkEnemyInformationsDataHandler : NetworkDataHandler {
    public NetworkEnemyInformationsDataHandler(NetworkGameController gameController) : base(gameController) { }
    public override void HandleMessage(string message) {
        string[] data = message.Split('|');

        foreach (var _enemyID in data) {
            int enemyID = int.Parse(_enemyID);

            if (gameController.EnemyDummies.ContainsKey(enemyID)) continue;

            gameController.SpawnEnemy(enemyID);
        }
    }
}