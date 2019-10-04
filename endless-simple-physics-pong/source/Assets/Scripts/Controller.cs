using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour {

	[SerializeField] private float gameplaySpeed = 1f;

	void Update() {
		Time.timeScale = gameplaySpeed;
	}
}
