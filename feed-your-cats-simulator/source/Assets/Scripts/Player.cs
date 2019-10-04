using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour {
	public static Player Instance { get; private set; }
	[SerializeField] private Camera _camera;

	[SerializeField] private float movementSpeed = 5;
	[SerializeField] private float cameraSpeed = 5;

	private int meat = 2;

	private void Awake() {
		if (Instance == null) {
			Instance = this;
		} else if (Instance != this) {
			Destroy(this);
		}
	}

	private void Start() {
		Cursor.visible = false;
	}

	private void Won() {
		Cursor.visible = true;
		HUD.Instance.ShowYouWon();
	}

	private void Update() {
		Movement();
		Watch();
		Respawn();
		GenerateNewWorld();
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

	private void Respawn() {
		if (transform.position.y <= -50) {
			transform.position = new Vector3(0, 2, 0);
		}
	}

	private void GenerateNewWorld() {
		if (Input.GetKeyDown(KeyCode.Space)) Restart();
	}

	public void Restart() {
		SceneManager.LoadScene(1);
	}

	private void OnCollisionEnter(Collision other) {
		if (!other.collider.CompareTag("Minion")) return;

		if (other.collider.GetComponent<Minion>().Feed()) {
			meat--;
			HUD.Instance.IncreaseHasFeeded();
			if (meat == 0) Won();
		}
	}
}
