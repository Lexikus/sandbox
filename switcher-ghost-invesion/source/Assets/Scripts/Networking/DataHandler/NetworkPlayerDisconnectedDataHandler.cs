public class NetworkPlayerDisconnectedDataHandler : NetworkDataHandler {
    public NetworkPlayerDisconnectedDataHandler(NetworkGameController gameController) : base(gameController) { }

    public override void HandleMessage(string message) {
        int connectionID = int.Parse(message);
        gameController.DespawnPlayer(connectionID);
    }
}