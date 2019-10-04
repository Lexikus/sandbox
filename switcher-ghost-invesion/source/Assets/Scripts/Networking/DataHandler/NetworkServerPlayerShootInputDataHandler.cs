using UnityEngine;

using InputKey = CharacterInput.InputKey;

public class NetworkServerPlayerShootInputDataHandler : NetworkDataHandler {
    public NetworkServerPlayerShootInputDataHandler(NetworkGameController gameController) : base(gameController) { }

    public override void HandleMessage(string message) {
        string[] data = message.Split('|');
        int playerID = int.Parse(data[0]);

        if (!gameController.Players.ContainsKey(playerID)) return;

        int _inputKey = int.Parse(data[1]);

        InputKey inputKey = (InputKey)_inputKey;
        Player player = gameController.Players[playerID];

        float degreeRotation = GetDegreeRotation(inputKey);

        Vector2 position = (Vector2)player.transform.position + OffsetFromDegree(degreeRotation);
        Quaternion rotation = Quaternion.AngleAxis(degreeRotation, Vector3.forward);

        gameController.SpawnDagger(position, rotation);
    }

    private Vector2 OffsetFromDegree(float degree) {
        if (degree == -90) return Vector2.right;
        else if (degree == -180) return Vector2.down;
        else if (degree == -270) return Vector2.left;
        else if (degree == -315) return Vector2.left + Vector2.up;
        else if (degree == -45) return Vector2.right + Vector2.up;
        else if (degree == -225) return Vector2.left + Vector2.down;
        else if (degree == -135) return Vector2.right + Vector2.down;
        return Vector2.up;
    }

    private float GetDegreeRotation(InputKey input) {
        float deg = 0;
        if (input == InputKey.ShootUp) deg = 0;
        else if (input == InputKey.ShootRight) deg = -90;
        else if (input == InputKey.ShootDown) deg = -180;
        else if (input == InputKey.ShootLeft) deg = -270;
        else if (input == InputKey.ShootUpperLeft) deg = -315;
        else if (input == InputKey.ShootUpperRight) deg = -45;
        else if (input == InputKey.ShootDownLeft) deg = -225;
        else if (input == InputKey.ShootDownRight) deg = -135;
        return deg;
    }
}