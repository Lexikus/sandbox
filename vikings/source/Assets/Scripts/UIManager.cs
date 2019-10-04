using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

	public static UIManager instance = null;

	[SerializeField] private GameObject GameOverScreen;
	[SerializeField] private GameObject PauseScreen;

	[SerializeField] private Text timer;
	[SerializeField] private Slider[] lifebars;
	[SerializeField] private Image[] powerbars;

	void Awake() {        
		// Singleton
        if (instance == null) {            
            instance = this;
        }                    
        else if (instance != this) {            
            Destroy(this);
        }                
    }        
    
    private void Start () {				
		InitLifebars();		
	}
	
	private void Update () {
		ObserveTimer();
		ObserveLife();
		ObservePower();
	}

	// Setzt die richtigen Paramter beim Slider
	private void InitLifebars(){
		lifebars[0].maxValue = GameManager.instance.GetViking(0).GetComponent<Viking>().Life;		
		lifebars[1].maxValue = GameManager.instance.GetViking(1).GetComponent<Viking>().Life;		
	}

	// Passt den Slider an anhand der Lebenspunkte der Wikinger
	private void ObserveLife(){		
		lifebars[0].value = GameManager.instance.GetViking(0).GetComponent<Viking>().Life;		
		lifebars[1].value = GameManager.instance.GetViking(1).GetComponent<Viking>().Life;
	}	

	// Manipuliert die Stärke Grafik
	private void ObservePower(){		
		powerbars[0].fillAmount = GameManager.instance.GetViking(0).GetComponent<Viking>().ForceP / 100;
		powerbars[1].fillAmount = GameManager.instance.GetViking(1).GetComponent<Viking>().ForceP / 100;
	}

	// Zeigt die restliche Zeit der Runde
	private void ObserveTimer(){
		int t = (int)GameManager.instance.Timer;
		timer.text = t.ToString();
	}

	public Slider[] Lifebar {
        get {
            return lifebars;
        }
    }

	// zeigt das Pause Menü
	public void ShowPause(){
		Time.timeScale = 0;
		PauseScreen.SetActive(true);
	}

	// zeigt das GameOver Menü
	public void ShowGameOver(){
		if(GameOverScreen.activeSelf){
			return;
		}
		Time.timeScale = 0;
		GameOverScreen.SetActive(true);
	}
}
