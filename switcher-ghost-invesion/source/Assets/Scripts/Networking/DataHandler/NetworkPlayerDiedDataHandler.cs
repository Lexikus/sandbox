using UnityEngine;

public class NetworkPlayerDiedDataHandler : NetworkDataHandler {
    public NetworkPlayerDiedDataHandler(NetworkGameController gameController) : base(gameController) { }
    public override void HandleMessage(string message) {
        int playerID = int.Parse(message);

        gameController.DespawnPlayer(playerID);

        Client client = gameController.Network as Client;
        if (client.Id == playerID) {
            DeathCamera.Instance.Enable();
        }
    }
}