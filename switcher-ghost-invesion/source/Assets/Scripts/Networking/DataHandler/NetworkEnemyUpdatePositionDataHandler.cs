using UnityEngine;

public class NetworkEnemyUpdatePositionDataHandler : NetworkDataHandler {
    public NetworkEnemyUpdatePositionDataHandler(NetworkGameController gameController) : base(gameController) { }
    public override void HandleMessage(string message) {
        string[] data = message.Split('|');

        foreach (var _enemyIDAndPosition in data) {
            string[] enemyIDAndPosition = _enemyIDAndPosition.Split('%');

            int enemyID = int.Parse(enemyIDAndPosition[0]);
            float x = float.Parse(enemyIDAndPosition[1]);
            float y = float.Parse(enemyIDAndPosition[2]);

            if (!gameController.EnemyDummies.ContainsKey(enemyID)) return;

            Vector2 beforePosition = gameController.EnemyDummies[enemyID].transform.position;
            Vector2 newPosition = new Vector2(x, y);

            gameController.EnemyDummies[enemyID].GetComponent<EnemyDummy>().SendIntervalRate = Network.GAME_TICK;
            gameController.EnemyDummies[enemyID].GetComponent<EnemyDummy>().NewPosition = newPosition;
        }
    }
}