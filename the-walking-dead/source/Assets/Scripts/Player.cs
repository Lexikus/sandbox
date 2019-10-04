using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    public static Player Instance { get; private set; }
    [SerializeField] private Camera _camera;

    [SerializeField] private float movementSpeed = 5;
    [SerializeField] private float cameraSpeed = 5;

    private bool isDead;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        } else if (Instance != this) {
            Destroy(this);
        }
    }

    private void Start() {
        isDead = false;
        Cursor.visible = false;
    }

    public void Kill() {
        isDead = true;
        Cursor.visible = true;
        HUD.Instance.ShowGameOver();
    }

    private void Won() {
        Cursor.visible = true;
        HUD.Instance.ShowYouWon();
    }

    private void Update() {
        if (isDead) return;
        Movement();
        Watch();
    }

    private void Movement() {
        Vector3 movement = Vector3.zero;

        movement.z = Input.GetAxis("Vertical");
        movement.x = Input.GetAxis("Horizontal");

        transform.Translate(movement.normalized * movementSpeed * Time.deltaTime, _camera.transform);
    }

    private void Watch() {
        Vector3 mouseAxis = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        _camera.transform.RotateAround(_camera.transform.position, _camera.transform.right, -mouseAxis.y * cameraSpeed);
        _camera.transform.RotateAround(_camera.transform.position, Vector3.up, mouseAxis.x * cameraSpeed);
    }

    private void OnTriggerEnter(Collider other) {
        if (!other.transform.CompareTag("Exit")) return;

        Won();
    }
}
