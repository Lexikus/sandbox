using UnityEngine;

public class NetworkEnemyDiedDataHandler : NetworkDataHandler {
    public NetworkEnemyDiedDataHandler(NetworkGameController gameController) : base(gameController) { }
    public override void HandleMessage(string message) {
        int enemyID = int.Parse(message);

        if (!gameController.EnemyDummies.ContainsKey(enemyID)) return;

        gameController.EnemyDummies[enemyID].GetComponent<EnemyDummy>().Die();
        gameController.DespawnEnemy(enemyID);
    }
}