using UnityEngine;

public class NetworkDaggerUpdatePositionDataHandler : NetworkDataHandler {
    public NetworkDaggerUpdatePositionDataHandler(NetworkGameController gameController) : base(gameController) { }
    public override void HandleMessage(string message) {
        string[] data = message.Split('|');

        foreach (var _daggerIDPositionAndRotation in data) {
            string[] daggerIDPositionAndRotation = _daggerIDPositionAndRotation.Split('%');

            int daggerID = int.Parse(daggerIDPositionAndRotation[0]);
            float x = float.Parse(daggerIDPositionAndRotation[1]);
            float y = float.Parse(daggerIDPositionAndRotation[2]);
            float rz = float.Parse(daggerIDPositionAndRotation[3]);

            if (!gameController.DaggerDummies.ContainsKey(daggerID)) return;

            Vector2 beforePosition = gameController.DaggerDummies[daggerID].transform.position;
            Quaternion beforeRotation = gameController.DaggerDummies[daggerID].transform.rotation;
            Vector2 newPosition = new Vector2(x, y);
            Quaternion newRotation = Quaternion.Euler(0, 0, rz);

            gameController.DaggerDummies[daggerID].GetComponent<DaggerDummy>().SendIntervalRate = Network.GAME_TICK;
            gameController.DaggerDummies[daggerID].GetComponent<DaggerDummy>().NewPosition = newPosition;
            gameController.DaggerDummies[daggerID].GetComponent<DaggerDummy>().NewRotation = newRotation;
        }
    }
}