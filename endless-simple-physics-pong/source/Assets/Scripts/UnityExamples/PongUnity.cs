using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PongUnity : MonoBehaviour {

	[SerializeField] private bool isLeftPlayer = true;
	[SerializeField] private float movementSpeed = 10f;

	private const float MAX_TOP_BOUNDARY = 13.5f;
	private const float MIN_TOP_BOUNDARY = 1.5f;
	private float EXPLISION_FORCE = 200;

	private void Update() {
		LeftPlayerControls();
		RightPlayerControls();
	}

	private void LeftPlayerControls() {
		if (!isLeftPlayer) return;
		if (Input.GetKey(KeyCode.W)) {
			if (transform.position.y >= MAX_TOP_BOUNDARY) return;
			transform.Translate(Vector3.left * Time.deltaTime * movementSpeed);
		} else if (Input.GetKey(KeyCode.S)) {
			if (transform.position.y <= MIN_TOP_BOUNDARY) return;
			transform.Translate(Vector3.right * Time.deltaTime * movementSpeed);
		} else if (Input.GetKey(KeyCode.D)) {
			BallUnity.instance.ActExplosionForce(EXPLISION_FORCE, 10, transform.position, BallUnity.instance.transform.position);
		}
	}

	private void RightPlayerControls() {
		if (isLeftPlayer) return;
		if (Input.GetKey(KeyCode.UpArrow)) {
			if (transform.position.y >= MAX_TOP_BOUNDARY) return;
			transform.Translate(Vector3.right * Time.deltaTime * movementSpeed);
		} else if (Input.GetKey(KeyCode.DownArrow)) {
			if (transform.position.y <= MIN_TOP_BOUNDARY) return;
			transform.Translate(Vector3.left * Time.deltaTime * movementSpeed);
		} else if (Input.GetKey(KeyCode.LeftArrow)) {
			BallUnity.instance.ActExplosionForce(EXPLISION_FORCE, 10, transform.position, BallUnity.instance.transform.position);
		}
	}

}
