using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pong : MonoBehaviour {

	public Trans _transform { get; private set; }

	private Renderer _renderer;

	[SerializeField] private bool isLeftPlayer = true;
	[SerializeField] private float movementSpeed = 10f;

	private const float MAX_TOP_BOUNDARY = 13.5f;
	private const float MIN_TOP_BOUNDARY = 1.5f;
	private const float EXPLOSION_FORCE = 1000;
	private const float EXPLOSION_COOLDOWN = 15f;
	private float explosionCooldown = 0;

	private readonly Color32 purple = new Color32(255, 0, 100, 1);

	private KeyCode upKey;
	private KeyCode downKey;
	private KeyCode explosionKey;

	private Action onExplosionReadyCallback;

	private void Start() {
		_transform = GetComponent<Trans>();
		_renderer = GetComponent<Renderer>();

		MapKeys();
	}

	private void Update() {
		PlayerControls();
		ManageCoolDown();
	}

	private void MapKeys() {
		if (isLeftPlayer) {
			upKey = KeyCode.W;
			downKey = KeyCode.S;
			explosionKey = KeyCode.D;
			return;
		}

		upKey = KeyCode.UpArrow;
		downKey = KeyCode.DownArrow;
		explosionKey = KeyCode.LeftArrow;
	}

	private void ManageCoolDown() {
		if (explosionCooldown <= 0) {
			explosionCooldown = 0;
			if (onExplosionReadyCallback != null) onExplosionReadyCallback();
			return;
		}
		explosionCooldown -= Time.deltaTime;
	}

	private void PlayerControls() {
		Vec3 direction = Vec3.Zero;

		if (Input.GetKey(upKey)) {
			if (_transform.Position.Y >= MAX_TOP_BOUNDARY) return;
			direction = Vec3.Up;
		} else if (Input.GetKey(downKey)) {
			if (_transform.Position.Y <= MIN_TOP_BOUNDARY) return;
			direction = Vec3.Down;
		}

		_transform.Translate(direction * Time.deltaTime * movementSpeed);

		if (Input.GetKeyDown(explosionKey)) {
			if (explosionCooldown > 0) return;
			Ball.Instance.ActExplosionForce(EXPLOSION_FORCE, 10, _transform.Position, Ball.Instance._transform.Position);
			SetExplosionCooldown();
		}
	}

	private void ChangeToPurple() {
		_renderer.material.SetColor("_Color", purple);
	}

	private void ChangeToWhite() {
		_renderer.material.SetColor("_Color", Color.white);
	}

	private void SetExplosionCooldown() {
		explosionCooldown = EXPLOSION_COOLDOWN;
		ChangeToWhite();
		onExplosionReadyCallback = OnExplosionReady;
	}

	public void ResetExplosionCooldown() {
		explosionCooldown = 0;
		ChangeToPurple();
		onExplosionReadyCallback = null;
	}

	private void OnExplosionReady() {
		ChangeToPurple();
		onExplosionReadyCallback = null;
	}
}
