using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {

    private GameObject owner;
    [SerializeField] private float damage = 10;
    private bool hasCollided = false;

    // Callback für den Wikinger
    private Action collided;
    
    private void Update() {
        CollideWhenOutOfScreen();
    }

    private void FixedUpdate() {
        ExternalForce();        
    }

    // Damit das Spiel nicht unnötig in die Länge gezogen wird, wird überprüft ob die Waffe aus der Sichtweite ist.
    private void CollideWhenOutOfScreen(){
        if(transform.position.y <= -10) {
            if (collided != null) {                
                collided();
            }
            Destroy(gameObject);
        }else if(transform.position.x <= CameraView.instance.MinPosition - 100){
            if (collided != null) {                
                collided();
            }
            Destroy(gameObject);
        }else if(transform.position.x >= CameraView.instance.MaxPosition + 100){
            if (collided != null) {                
                collided();
            }
            Destroy(gameObject);
        }
    }    

    // Wind beinflusst die Wurfstärke
    private void ExternalForce(){
        WindZone w = GameManager.instance.Wind;
        Rigidbody r = GetComponent<Rigidbody>();
        r.AddForce(w.transform.forward * w.windMain * 2.5f, ForceMode.Force); 
    }    

    // Kollisionshandling
    private void OnCollisionEnter(Collision collision) {
        // Falls es mit irgendwas mal kollidiert hat, soll es keine Überprüfungen mehr machen.
        if (hasCollided) {
            return;
        }
        
        // Kann nicht mit sich selbst kollidieren (Sicherheitshalber)
        if (collision.gameObject == gameObject) {
            return;
        }
        
        // Kann nicht mit dem Werfer kollidieren (Sicherheitshalber)
        if (collision.transform.root.gameObject == owner) {
            return;
        }

        // Andere Waffen sollen ignoriert werden.
        if(collision.gameObject.tag == "Weapon"){
            if(collision.gameObject.GetComponent<Weapon>().HasCollided){
                return;
            }
        }

        // Falls es mit dem Gegner kollidiert, soll es Schaden abziehen
        if(collision.gameObject.tag == "Player") {
            collision.gameObject.GetComponent<Viking>().TakeDamage(damage);            
        }    
        
                
        if (collided != null) {            
            collided();
        }
        hasCollided = true;        
    }

    public Action Collided {
        get {
            return collided;
        }

        set {
            collided = value;
        }
    }

    public GameObject Owner {        
        set {
            owner = value;
        }
    }

    public bool HasCollided {
        get {
            return hasCollided;
        }
    }
}
