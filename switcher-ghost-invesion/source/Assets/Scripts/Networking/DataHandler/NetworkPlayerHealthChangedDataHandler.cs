using UnityEngine;

public class NetworkPlayerHealthChangedDataHandler : NetworkDataHandler {
    public NetworkPlayerHealthChangedDataHandler(NetworkGameController gameController) : base(gameController) { }
    public override void HandleMessage(string message) {
        string[] data = message.Split('|');

        int playerID = int.Parse(data[0]);
        int health = int.Parse(data[1]);

        if (!gameController.PlayerDummies.ContainsKey(playerID)) return;

        gameController.PlayerDummies[playerID].GetComponent<PlayerDummy>().ChangeHealth(health);
    }
}