using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {

	void Start() {
		SphereCollider collider = gameObject.AddComponent<SphereCollider>();
		collider.radius = 1.8f;
		collider.isTrigger = true;
	}

	private void OnTriggerEnter(Collider other) {
		if (!other.CompareTag("Player")) return;
		HideDoor();

	}

	private void OnTriggerExit(Collider other) {
		if (!other.CompareTag("Player")) return;
		ShowDoor();
	}

	private void HideDoor() {
		foreach (Transform item in transform) {
			item.gameObject.SetActive(false);
		}
	}

	private void ShowDoor() {
		foreach (Transform item in transform) {
			item.gameObject.SetActive(true);
		}
	}
}
