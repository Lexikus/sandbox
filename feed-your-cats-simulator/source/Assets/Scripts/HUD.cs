using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour {

	public static HUD Instance { get; private set; }

	[SerializeField] Text hasFeededText;
	[SerializeField] GameObject youWonPanel;

	private void Awake() {
		if (Instance == null) {
			Instance = this;
		} else if (Instance != this) {
			Destroy(this);
		}
	}

	public void IncreaseHasFeeded() {
		int hasFeeded = int.Parse(hasFeededText.text) + 1;
		hasFeededText.text = hasFeeded.ToString();
	}

	public void ShowYouWon() {
		youWonPanel.SetActive(true);
	}
}
