using UnityEngine;

public class Pressure : MonoBehaviour {

	private AudioSource audioSource;

	private void Start() {
		GetComponent<BoxCollider>().isTrigger = true;

		audioSource = gameObject.AddComponent<AudioSource>();

		AudioClip audioClip = Resources.Load("For_love_and_hate_ft._Skandor") as AudioClip;
		audioSource.clip = audioClip;
		audioSource.playOnAwake = false;
		audioSource.loop = true;
	}

	private void OnTriggerEnter(Collider collider) {
		if (!collider.CompareTag("Player")) return;
		audioSource.Play();
	}

	private void OnTriggerExit(Collider collider) {
		if (!collider.CompareTag("Player")) return;
		audioSource.Pause();
	}
}