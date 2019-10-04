using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour {
    
    // Startet das Hauptspiel
    public void StartGame() {
        Time.timeScale = 1;
        SceneManager.LoadSceneAsync("Game", LoadSceneMode.Single);
    }

    // Fortsetzt das Spiel
    public void ResumeGame(){
        Time.timeScale = 1;
    }

    // Schliesst das Spiel
    public void CloseGame(){
        Application.Quit();
    }
}
