using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInput : MonoBehaviour {
    public enum InputKey {

        ShootUp,
        ShootRight,
        ShootLeft,
        ShootDown,
        ShootUpperLeft,
        ShootUpperRight,
        ShootDownLeft,
        ShootDownRight,

        MoveIdle,
        MoveUp,
        MoveRight,
        MoveLeft,
        MoveDown
    }

    [SerializeField] private CharacterInputKeyEvent inputEvent;
    [SerializeField] private CharacterInputMovementEvent inputMovementEvent;

    private KeyCode shootUp;
    private KeyCode shootRight;
    private KeyCode shootLeft;
    private KeyCode shootDown;
    private KeyCode shootUpperLeft;
    private KeyCode shootUpperRight;
    private KeyCode shootDownLeft;
    private KeyCode shootDownRight;

    private KeyCode moveUp;
    private KeyCode moveRight;
    private KeyCode moveLeft;
    private KeyCode moveDown;

    private void Start() {
        InitKeyCodes();
    }

    private void InitKeyCodes() {
        moveUp = KeyCode.UpArrow;
        moveRight = KeyCode.RightArrow;
        moveLeft = KeyCode.LeftArrow;
        moveDown = KeyCode.DownArrow;

        shootUp = KeyCode.W;
        shootRight = KeyCode.D;
        shootLeft = KeyCode.A;
        shootDown = KeyCode.S;
        shootUpperLeft = KeyCode.Q;
        shootUpperRight = KeyCode.E;
        shootDownLeft = KeyCode.Y;
        shootDownRight = KeyCode.C;
    }

    private void Update() {
        HandleShoot();
    }

    private void FixedUpdate() {
        HandleMovement();
    }

    private void HandleShoot() {
        if (Input.GetKeyDown(shootUp)) inputEvent.Invoke(InputKey.ShootUp);
        else if (Input.GetKeyDown(shootRight)) inputEvent.Invoke(InputKey.ShootRight);
        else if (Input.GetKeyDown(shootDown)) inputEvent.Invoke(InputKey.ShootDown);
        else if (Input.GetKeyDown(shootLeft)) inputEvent.Invoke(InputKey.ShootLeft);
        else if (Input.GetKeyDown(shootUpperLeft)) inputEvent.Invoke(InputKey.ShootUpperLeft);
        else if (Input.GetKeyDown(shootUpperRight)) inputEvent.Invoke(InputKey.ShootUpperRight);
        else if (Input.GetKeyDown(shootDownLeft)) inputEvent.Invoke(InputKey.ShootDownLeft);
        else if (Input.GetKeyDown(shootDownRight)) inputEvent.Invoke(InputKey.ShootDownRight);
    }

    private void HandleMovement() {
        Vector2 movement = Vector2.zero;
        InputKey inputKey = InputKey.MoveIdle;

        if (Input.GetKey(moveLeft)) {
            movement += Vector2.left;
            inputKey = InputKey.MoveLeft;
        }

        if (Input.GetKey(moveUp)) {
            movement += Vector2.up;
            inputKey = InputKey.MoveUp;
        }

        if (Input.GetKey(moveRight)) {
            movement += Vector2.right;
            inputKey = InputKey.MoveRight;
        }

        if (Input.GetKey(moveDown)) {
            movement += Vector2.down;
            inputKey = InputKey.MoveDown;
        }

        inputMovementEvent.Invoke(movement, inputKey);
    }

    public static InputKey Vector2ToInputKey(Vector2 movement) {
        float x = movement.x;
        float y = movement.y;
        if (x < 0) x = -1;
        if (x > 0) x = 1;
        if (y < 0) y = -1;
        if (y > 0) y = 1;
        movement = new Vector2(x, y);

        if (movement == Vector2.left) {
            return InputKey.MoveLeft;
        }
        else if (movement == Vector2.up) {
            return InputKey.MoveUp;
        }
        else if (movement == Vector2.right) {
            return InputKey.MoveRight;
        }
        else if (movement == Vector2.down) {
            return InputKey.MoveDown;
        }
        else if (movement == Vector2.up + Vector2.left) {
            return InputKey.MoveUp;
        }
        else if (movement == Vector2.up + Vector2.right) {
            return InputKey.MoveUp;
        }
        else if (movement == Vector2.down + Vector2.left) {
            return InputKey.MoveDown;
        }
        else if (movement == Vector2.down + Vector2.right) {
            return InputKey.MoveDown;
        }

        return InputKey.MoveIdle;
    }
}
