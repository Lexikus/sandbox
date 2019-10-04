using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using InputKey = CharacterInput.InputKey;

public class CharacterMovement : MonoBehaviour {
    [SerializeField] private float speed = 5;
    private Vector2 movement;

    public void HandleMovement(Vector2 movement) {
        this.movement = movement.normalized;
    }

    private void FixedUpdate() {
        transform.Translate(movement * speed * Time.deltaTime);
    }
}