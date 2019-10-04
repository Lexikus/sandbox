using UnityEngine;

public class NetworkDaggerInformationsDataHandler : NetworkDataHandler {
    public NetworkDaggerInformationsDataHandler(NetworkGameController gameController) : base(gameController) { }
    public override void HandleMessage(string message) {
        string[] data = message.Split('|');

        foreach (var _daggerIDPosAndDeg in data) {
            string[] daggerIDPosAndDeg = _daggerIDPosAndDeg.Split('%');

            int daggerID = int.Parse(daggerIDPosAndDeg[0]);

            if (gameController.DaggerDummies.ContainsKey(daggerID)) continue;

            float x = float.Parse(daggerIDPosAndDeg[1]);
            float y = float.Parse(daggerIDPosAndDeg[2]);
            float rz = float.Parse(daggerIDPosAndDeg[3]);

            Vector2 position = new Vector2(x, y);
            Quaternion rotation = Quaternion.Euler(0, 0, rz);

            gameController.SpawnDagger(daggerID, position, rotation);
        }
    }
}