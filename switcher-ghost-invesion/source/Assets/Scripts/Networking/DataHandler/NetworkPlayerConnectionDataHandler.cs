using UnityEngine;

public class NetworkPlayerConnectionDataHandler : NetworkDataHandler {
    public NetworkPlayerConnectionDataHandler(NetworkGameController gameController) : base(gameController) { }

    public override void HandleMessage(string message) {
        string[] data = message.Split('|');
        Client client = gameController.Network as Client;

        foreach (var _connectionID in data) {
            int connectionID = int.Parse(_connectionID);

            if (gameController.PlayerDummies.ContainsKey(connectionID)) continue;

            GameObject player = gameController.SpawnPlayer(connectionID);

            if (connectionID == client.Id) {
                player.transform.name = player.transform.name + " - ME";
                PlayerDummy dummy = player.GetComponent<PlayerDummy>();
                dummy.ShowCamera();
                dummy.ShowHUD();
            }
        }
    }
}