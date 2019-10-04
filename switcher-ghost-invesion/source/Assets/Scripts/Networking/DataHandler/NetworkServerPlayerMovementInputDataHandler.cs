using UnityEngine;

public class NetworkServerPlayerMovementInputDataHandler : NetworkDataHandler {
    public NetworkServerPlayerMovementInputDataHandler(NetworkGameController gameController) : base(gameController) { }
    public override void HandleMessage(string message) {
        string[] data = message.Split('|');
        int playerID = int.Parse(data[0]);

        if (!gameController.Players.ContainsKey(playerID)) return;

        string[] movementVector = data[1].Split('%');

        float x = float.Parse(movementVector[0]);
        float y = float.Parse(movementVector[1]);
        float inputKey = byte.Parse(movementVector[2]);

        Vector2 movement = new Vector2(x, y);

        gameController.Players[playerID].SetMovement(movement, (CharacterInput.InputKey)inputKey);
    }
}