using UnityEngine;

public class Volume : MonoBehaviour {
	private Light light;

	private void Start() {
		light = gameObject.AddComponent<Light>();
		light.enabled = false;
		light.intensity = 2;
	}

	private void OnTriggerEnter(Collider other) {
		if (!other.CompareTag("Player")) return;
		light.enabled = true;
	}

	private void OnTriggerExit(Collider other) {
		if (!other.CompareTag("Player")) return;
		light.enabled = false;
	}
}