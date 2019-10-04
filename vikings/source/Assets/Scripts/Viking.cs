using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Viking : MonoBehaviour {

    [SerializeField] private float life = 100f;
    
    [SerializeField] private bool lefthanded = false;
    
    private float angle = 0f;
    [SerializeField] private GameObject angleArrow;
    private float angleSpeed = 50f;

    private float minForce = 4000f;
    private float maxForce = 13000f;
    private float force = 1900f;

    [SerializeField] private GameObject[] weaponSlots;
    [SerializeField] private GameObject weapon;

    [SerializeField] private bool isActive = false;
    private int state = 0; 

    private Animator anim;
    [SerializeField] private AudioClip takeDamageSFX;
    [SerializeField] private AudioClip throwSFX;

    // Use this for initialization
    private void Start() {
        anim = GetComponent<Animator>();                        
    }    

    private void Update() {
        PrepareToThrow();
        ObserveAngle();
    }

    // Je nach Phase holt eine neue Waffe, berechnet den Wurfwinkel, berechnet die Wurfkraft und startet die Animation
    private void PrepareToThrow() {    
        if (!isActive) {
            return;
        }        
        switch (state) {
            case 0: GetWeapon(); break;
            case 1: CalibrateAngle(); break;
            case 2: CalibrateForce(); break;
            case 3: MoveHand(); break;
        }
    }

    // Holt eine Waffe aus dem Pool
    private void GetWeapon() {
        GameObject w = GameManager.instance.GetWeapon();        
        if(lefthanded){
            w.transform.parent = weaponSlots[0].transform;
        }else{
            w.transform.parent = weaponSlots[1].transform;
        }
        w.transform.localPosition = Vector3.zero;
        w.transform.localRotation = Quaternion.identity;
        weapon = w;

        weapon.GetComponent<Weapon>().Collided += TurnOver;
        weapon.GetComponent<Weapon>().Owner = gameObject;

        state = 1;
    }

    // Startet die Animation und den SFX
    private void MoveHand() {
        if (lefthanded) {
            anim.SetTrigger("ThrowLeft");
        } else {
            anim.SetTrigger("ThrowRight");
        }
        state = 4;
        GameManager.instance.TimerStatus = false;
        PlaySFX(throwSFX);
    }    

    // Berechnet den Winkel
    private void CalibrateAngle() {
        // Falls auf ein UI element geklickt wird, soll gar nichts passieren!
        if (Input.GetMouseButton(0) || Input.GetMouseButtonDown(0) || Input.GetMouseButtonUp(0)) {
            if (EventSystem.current.IsPointerOverGameObject()) {                
                return;
            }
        }

        if (Input.GetMouseButton(0)) {
            angle = Mathf.Clamp(angle += Time.deltaTime * angleSpeed, 0f, 90f);
        } else if(Input.GetMouseButtonUp(0)) {
            state = 2;
        }        
    }

    // Zeigt den Winkel im Spiel an
    private void ObserveAngle(){        
        angleArrow.transform.rotation = Quaternion.Euler(angleArrow.transform.eulerAngles.x, angleArrow.transform.eulerAngles.y, angle);
    }

    // Berechnet die Wurfkraft
    private void CalibrateForce() {
        // Falls auf ein UI element geklickt wird, soll gar nichts passieren!
        if (Input.GetMouseButton(0) || Input.GetMouseButtonDown(0) || Input.GetMouseButtonUp(0)) {
            if (EventSystem.current.IsPointerOverGameObject()) {                
                return;
            }
        }

        if (Input.GetMouseButton(0)) {
            force = Mathf.Clamp(force + Time.deltaTime * 3000, minForce, maxForce);
        } else if (Input.GetMouseButtonUp(0)) {
            state = 3;
        }
    }

    // Wirft wie Waffe
    private void Throw() {       
        weapon.transform.parent = null;

        CameraView.instance.FollowTo = weapon;

        Rigidbody rb = weapon.GetComponent<Rigidbody>();
        rb.isKinematic = false;        
        foreach(Collider c in weapon.GetComponentsInChildren<Collider>()) {            
            c.enabled = true;
        }

        Vector3 dir = Quaternion.AngleAxis(angle, Vector3.left) * Vector3.forward;        
        rb.AddRelativeForce(dir * force);
        rb.AddRelativeTorque(Vector3.right * force);
    }    

    // Callback falls die Waffe mit irgendwas kollidiert ist.
    private void TurnOver() {
        CameraView.instance.FollowTo = null;
        weapon.GetComponent<Weapon>().Collided -= TurnOver;
        state = 5;
    }

    // OneShot SFX
    private void PlaySFX(AudioClip clip){
        GetComponent<AudioSource>().PlayOneShot(clip);
    }

    // Fügt sich selber Schaden
    public void TakeDamage(float d) {
        life -= d;
        PlaySFX(takeDamageSFX);
    }

    // Setzt gewisse Paramtern zurück
    private void ResetSelf() {
        state = 0;
        angle = 0f;
        force = 1700f;
        foreach (var item in weaponSlots)
        {
            foreach (var subitem in item.GetComponentsInChildren<Weapon>())
            {
                Destroy(subitem.gameObject);
            } 
        }
    }

    // Überprüft ob der Spieler tod ist.
    public bool IsDead {
        get {
            if (life <= 0) {
                return true;
            }
            return false;
        }
    }

    // Aktiviert den Wikinger
    public bool IsActive {
        get {
            return isActive;
        }

        set {
            if (!value) {
                ResetSelf();
            }
            isActive = value;
        }
    }

    public int State {
        get {
            return state;
        }
    }

    public float Life {
        get {
            return life;
        }
    }

    public float ForceP {
        get {
            return 100f / (maxForce-minForce) * (force - minForce);
        }
    }
}
