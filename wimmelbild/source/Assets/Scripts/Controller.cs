using UnityEngine;
using UnityEngine.SceneManagement;

public class Controller : MonoBehaviour
{
    [SerializeField] private GameObject winCanvas;
    private int missingElements = 10;

    public static Controller Instance { get; private set; }

    private void Awake() {
        if (Instance == null) Instance = this;
        else if (Instance != this) Destroy(this);
    }

    public void TriggerFoundItem() {
        missingElements--;

        if(missingElements < 1) {
            ShowWinningCanvas();
        }
    }

    public void ShowWinningCanvas() {
        winCanvas.SetActive(true);
    }

    public void ReloadGame() {
        SceneManager.LoadScene("Game");
    }
}
