using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleUnity : MonoBehaviour {

	private const float IGNORE_COLLISION_TIMER = 0.2f;
	private float ignoreCollisionTimer = 0;
	public bool IgnoreCollision { get; private set; }

	[SerializeField] private float radius;

	public float Radius { get { return radius; } }

	private void Start() {
		IgnoreCollision = false;
	}

	private void Update() {
		ManageCollisionTimer();
	}

	private void ManageCollisionTimer() {
		if (ignoreCollisionTimer > 0) {
			ignoreCollisionTimer -= Time.deltaTime;

			if (IgnoreCollision) return;

			IgnoreCollision = true;
		} else {
			if (!IgnoreCollision) return;
			IgnoreCollision = false;
			ignoreCollisionTimer = 0;
		}
	}

	private void SetCollisionTimer() {
		ignoreCollisionTimer = IGNORE_COLLISION_TIMER;
	}

	internal void SetCollided() {
		SetCollisionTimer();
	}
}
