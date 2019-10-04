using System;
using UnityEngine;
using UnityEngine.Networking;

public class Dagger : MonoBehaviour {
    private float speed = 10f;
    [SerializeField] private float lifetime = 2f;

    public int Id { get; set; }
    public Action<int> OnBreak { private get; set; }

    private void Start() {
        Invoke("Break", lifetime);
    }

    private void FixedUpdate() {
        transform.Translate(GetDirectionFromRotation(transform.rotation.eulerAngles) * speed * Time.fixedDeltaTime, Space.Self);
    }

    private Vector2 GetDirectionFromRotation(Vector3 euler) {
        if (euler.z == -45f) return Vector2.up;
        if (euler.z == -90f) return Vector2.right;
        if (euler.z == -135f) return Vector2.up;
        if (euler.z == -180f) return Vector2.down;
        if (euler.z == -225f) return Vector2.up;
        if (euler.z == -270f) return Vector2.left;
        if (euler.z == -315f) return Vector2.up;
        return Vector2.up;
    }

    private void OnCollisionEnter2D(Collision2D other) {
        speed = speed / 2;

        if (other.transform.CompareTag("Enemy")) {
            OnCollidingEnemy(other.gameObject);
        }
    }

    private void OnCollidingEnemy(GameObject _enemy) {
        Break();

        Enemy enemy = _enemy.GetComponent<Enemy>();
        enemy.TakeDamage();
    }

    private void Break() {
        if (OnBreak != null) OnBreak(Id);
    }
}
