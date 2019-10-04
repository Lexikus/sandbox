using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour {

	public static HUD Instance { get; private set; }

	[SerializeField] private Text LeftPoint;
	[SerializeField] private Text RightPoint;

	private void Awake() {
		if (Instance == null) {
			Instance = this;
		} else if (Instance != this) {
			Destroy(this);
		}
	}

	public void AddPointLeft() {
		LeftPoint.text = (int.Parse(LeftPoint.text) + 1).ToString();
	}
	public void AddPointRight() {
		RightPoint.text = (int.Parse(RightPoint.text) + 1).ToString();
	}
}
