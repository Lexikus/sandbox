using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HUD : MonoBehaviour {

    public static HUD Instance { get; private set; }
    [SerializeField] private Canvas canvas;
    [SerializeField] private Text gameOver;
    [SerializeField] private Text youWon;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        } else if (Instance != this) {
            Destroy(this);
        }
    }

    public void ShowGameOver() {
        gameOver.gameObject.SetActive(true);
        canvas.gameObject.SetActive(true);
    }

    public void ShowYouWon() {
        youWon.gameObject.SetActive(true);
        canvas.gameObject.SetActive(true);
    }

    public void Restart() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
