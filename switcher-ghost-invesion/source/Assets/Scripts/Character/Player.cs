using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

using InputKey = CharacterInput.InputKey;

[RequireComponent(typeof(CharacterMovement))]
[RequireComponent(typeof(CharacterAnimation))]
public class Player : MonoBehaviour {
    [SerializeField] private int initialHealth = 10;
    private int health;

    private CharacterMovement characterMovement;
    private CharacterAnimation characterAnimation;

    public Action<int> OnDying { private get; set; }
    public Action<int, int> OnHealthChange { private get; set; }
    public InputKey LastMovementInputKey { get; private set; }

    public int Id { get; set; }

    public bool IsDead {
        get { return health <= 0; }
    }

    private void Start() {
        health = initialHealth;
        LastMovementInputKey = InputKey.MoveIdle;
        characterMovement = GetComponent<CharacterMovement>();
        characterAnimation = GetComponent<CharacterAnimation>();
    }

    public void TakeDamage() {
        if (health <= 0) return;

        health--;
        if (OnHealthChange != null) OnHealthChange(Id, health);

        if (health == 0) Die();
    }

    public void SetMovement(Vector2 movement, InputKey inputKey) {
        LastMovementInputKey = inputKey;
        characterMovement.HandleMovement(movement);
        characterAnimation.HandleMovementAnimaton(inputKey);
    }

    private void Die() {
        if (OnDying != null) OnDying(Id);
        // Destroy(gameObject);
    }
}
