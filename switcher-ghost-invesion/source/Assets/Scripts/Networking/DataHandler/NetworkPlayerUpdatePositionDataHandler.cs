using UnityEngine;

public class NetworkPlayerUpdatePositionDataHandler : NetworkDataHandler {
    public NetworkPlayerUpdatePositionDataHandler(NetworkGameController gameController) : base(gameController) { }
    public override void HandleMessage(string message) {
        string[] data = message.Split('|');

        foreach (var _playerIDAndPosition in data) {
            string[] playerIDAndPosition = _playerIDAndPosition.Split('%');

            int playerID = int.Parse(playerIDAndPosition[0]);
            float x = float.Parse(playerIDAndPosition[1]);
            float y = float.Parse(playerIDAndPosition[2]);
            int inputKey = int.Parse(playerIDAndPosition[3]);

            if (!gameController.PlayerDummies.ContainsKey(playerID)) return;

            Vector2 beforePosition = gameController.PlayerDummies[playerID].transform.position;
            Vector2 newPosition = new Vector2(x, y);

            Vector2 direction = (newPosition - beforePosition).normalized;

            gameController.PlayerDummies[playerID].GetComponent<CharacterAnimation>().HandleMovementAnimaton((CharacterInput.InputKey)inputKey);

            gameController.PlayerDummies[playerID].GetComponent<PlayerDummy>().SendIntervalRate = Network.GAME_TICK;
            gameController.PlayerDummies[playerID].GetComponent<PlayerDummy>().NewPosition = newPosition;

        }
    }
}