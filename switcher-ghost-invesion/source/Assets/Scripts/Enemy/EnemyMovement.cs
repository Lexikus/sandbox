using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class EnemyMovement : MonoBehaviour {

    public Transform Target { get; set; }

    [SerializeField] private float speed = 1f;
    private const float DIFF = 0.2f;

    private void FixedUpdate() {
        HandleMovement();
    }

    private void HandleMovement() {
        if (Target == null) return;

        Vector2 direction = (Vector2)Target.position - (Vector2)transform.position;

        if (direction.magnitude <= DIFF) return;

        direction = direction.normalized;
        transform.Translate(direction * speed * Time.deltaTime);
    }
}
