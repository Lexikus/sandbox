using UnityEngine;

public class NetworkPlayerIdentifierDataHandler : NetworkDataHandler {
    public NetworkPlayerIdentifierDataHandler(NetworkGameController gameController) : base(gameController) { }

    public override void HandleMessage(string message) {
        int clientID = int.Parse(message);

        if (!gameController.PlayerDummies.ContainsKey(clientID)) return;

        // In case the identifier arrives after the player connections
        Camera camera = gameController.PlayerDummies[clientID].GetComponentInChildren<Camera>();
        camera.enabled = true;
    }
}