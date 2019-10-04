using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

using InputKey = CharacterInput.InputKey;

[RequireComponent(typeof(Animator))]
public class CharacterAnimation : MonoBehaviour {
    [SerializeField] private Animator _animator;

    private void Start() {
        _animator = GetComponent<Animator>();
    }

    public void HandleMovementAnimaton(InputKey input) {
        if (_animator == null) return;
        if (input == InputKey.MoveLeft) _animator.SetInteger("Walk", 4);
        else if (input == InputKey.MoveUp) _animator.SetInteger("Walk", 1);
        else if (input == InputKey.MoveRight) _animator.SetInteger("Walk", 2);
        else if (input == InputKey.MoveDown) _animator.SetInteger("Walk", 3);
        else _animator.SetInteger("Walk", 0);
    }

    public void HandleMovementAnimaton(Vector2 movement) {
        InputKey inputkey = CharacterInput.Vector2ToInputKey(movement);
        HandleMovementAnimaton(inputkey);
    }
}