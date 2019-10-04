using UnityEngine;

public class NetworkDaggerBrokeDataHandler : NetworkDataHandler {
    public NetworkDaggerBrokeDataHandler(NetworkGameController gameController) : base(gameController) { }
    public override void HandleMessage(string message) {
        int daggerID = int.Parse(message);

        if (!gameController.DaggerDummies.ContainsKey(daggerID)) return;

        gameController.DespawnDagger(daggerID);
    }
}