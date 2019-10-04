using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager instance = null;

    [SerializeField] private GameObject[] vikings;
    [SerializeField] private GameObject[] weapons;
    [SerializeField] private WindZone wind;

    private int round = 0;
    private float timer = 20.9f;
    private bool timerStatus = true;

    [SerializeField] private float physicsMultiplier = 1f;    
    
    void Awake() {
        // Singleton
        if (instance == null) {            
            instance = this;
        }                    
        else if (instance != this) {            
            Destroy(this);
        }        
    }               
    private void Start() {
        Physics.gravity = Physics.gravity * physicsMultiplier;
    }
		
	private void Update () {
        ActivateViking();
        TimerCountdown();
        IsGameOver();
    }

    // Überprüft ob jemand gestorben ist und zeigt das GameOver Screen
    private void IsGameOver(){
        if(IsAVikingDeath()){
            UIManager.instance.ShowGameOver();
        }
    }

    // Überprüft ob jemand gestorben ist
    private bool IsAVikingDeath(){
        foreach (var v in vikings) {
            if(v.GetComponent<Viking>().IsDead){
                return true;
            }
        }
        return false;
    }

    // Countdown pro Runde
    private void TimerCountdown(){
        if(!timerStatus){
            return;
        }
        timer -= Time.deltaTime;
        if(timer<=0){
            timer = 0;
        }
    }

    // Aktiviert ein Wikinger Abwechslungsweise
    private void ActivateViking() {
        if(vikings.Length != 2) {
            return;
        }

        Viking v = vikings[round%2].GetComponent<Viking>();

        if (!v.IsActive) {
            v.IsActive = true;
            
            if(v.transform.position.x < 0){
                CameraView.instance.MoveTo = Direction.Left;
            }else{
                CameraView.instance.MoveTo = Direction.Right;
            }
            
        } else {
            if (v.State >= 5) {
                ResetViking(v);
            }
        }

        if(timer<=0){
            ResetViking(v);
        }
    }

    // Resetet gewisse Parameter für die neue Runde
    private void ResetViking(Viking v){
        v.IsActive = false;
        timer = 20.9f;
        timerStatus = true;
        round++;
        ChangeWindDirection();
    }

    // Holt eine Waffe aus dem Pool
    public GameObject GetWeapon() {
        return Instantiate(weapons[Random.Range(0, weapons.Length)], Vector3.zero, Quaternion.identity);
    }

    // Gibt ein Wikinger zurück
    public GameObject GetViking(int index){
        return vikings[index];
    }

    // Verändert die Windstärke und Richtung
    private void ChangeWindDirection(){
        if(Random.Range(0,100) % 2 == 0){
            wind.transform.rotation = Quaternion.Euler(0,90,0);
        }else{
            wind.transform.rotation = Quaternion.Euler(0,-90,0);
        }
        wind.windMain = Random.Range(0f, 10f);
    }

    public float Timer {
        get {
            return timer;
        }
    }

    public bool TimerStatus {    
        set {
            timerStatus = value;
        }
    }

    public WindZone Wind {
        get {
            return wind;
        }
    }
}
