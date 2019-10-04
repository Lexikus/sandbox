using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

using AnimationType = EnemyAnimation.AnimationType;

[RequireComponent(typeof(EnemyMovement))]
[RequireComponent(typeof(EnemyAnimation))]
public class Enemy : MonoBehaviour {
    [SerializeField] private int initialHealth = 10;
    private int health = 10;

    private EnemyMovement enemyMovement;
    private EnemyAnimation enemyAnimation;

    public Action<int> OnDying { private get; set; }

    public int Id { get; set; }

    // we need the references before the first frame.
    private void Awake() {
        enemyMovement = GetComponent<EnemyMovement>();
        enemyAnimation = GetComponent<EnemyAnimation>();
    }

    private void Start() {
        health = initialHealth;
    }

    public void SetTarget(Transform target) {
        enemyMovement.Target = target;
    }

    public void SetAnimation(AnimationType animationType) {
        enemyAnimation.HandleAnimation(animationType);
    }

    public void TakeDamage() {
        if (health <= 0) return;

        health--;

        if (health == 0) Die();
    }

    private void Die() {
        health = 0;

        enemyAnimation.HandleAnimation(AnimationType.Die);

        enemyMovement.Target = null;

        if (OnDying != null) OnDying(Id);
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.transform.CompareTag("Player")) {
            OnCollidingPlayer(other.gameObject);
        }
    }

    private void OnCollidingPlayer(GameObject _player) {
        if (health <= 0) return;
        Player player = _player.GetComponent<Player>();
        player.TakeDamage();
        Die();
    }
}
