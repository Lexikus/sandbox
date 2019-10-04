using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction{
	Stay,
	Left,
	Right
}

public class CameraView : MonoBehaviour {

	public static CameraView instance = null;
	private float lastPosition = 0;
	private float minPosition = -190;
	private float maxPosition = 190;
	private Direction moveTo = Direction.Stay;
	private float moveToSpeed = 1000f;
	private GameObject followTo = null;
	private float followToSpeed = 500f;

    private void Awake() {
       //Singleton
        if (instance == null) {            
            instance = this;
        }                    
        else if (instance != this) {            
            Destroy(this);
        }
    }

	private void Update() {
		MoveToGivenDirection();
		FollowToGameObject();
	}

	// Verfolgt die geworfene Waffe
    private void FollowToGameObject() {
		if(followTo == null){
			return;
		}

        if(moveTo != Direction.Stay){
			moveTo = Direction.Stay;
		}

		transform.position = Vector3.MoveTowards(transform.position, new Vector3(followTo.transform.position.x, 30f, -140), Time.deltaTime * followToSpeed);
    }

    private void LateUpdate() {
		//MoveCamera();
		ClampCamera();
	}

	// Springt zum richtigen Wikinger
	private void MoveToGivenDirection(){
		if(moveTo == Direction.Left){
			transform.position = Vector3.MoveTowards(transform.position, new Vector3(minPosition, 30f, -140f), Time.deltaTime * moveToSpeed);
			if(transform.position.x <= minPosition){
				moveTo = Direction.Stay;
			}
		}else if(moveTo == Direction.Right){
			transform.position = Vector3.MoveTowards(transform.position, new Vector3(maxPosition, 30f, -140f), Time.deltaTime * moveToSpeed);
			if(transform.position.x >= maxPosition){
				moveTo = Direction.Stay;
			}
		}
	}

	// Fixiert die Kamera zwüschen 2 gegebenen Positionen
	private void ClampCamera(){
		if(transform.position.x <= minPosition){
			transform.position = new Vector3(minPosition, transform.position.y, transform.position.z);
		}else if(transform.position.x >= maxPosition){
			transform.position = new Vector3(maxPosition, transform.position.y, transform.position.z);
		}
	}

	// Bewegt die Kamera. Ist aber nicht aktiv im Spiel, da es meiner Meinung nicht nötig ist.
	private void MoveCamera(){
		if((Input.GetMouseButton(0) && Input.GetMouseButton(1)) || Input.touchCount >= 2){
			Vector3 listener = Input.mousePosition;
			if(Input.touchCount > 0){
				listener = Input.touches[0].position;
			}
			
			float currentPosition  = listener.x;
			if(lastPosition == 0){
				lastPosition = currentPosition;	
			}

			float diffDelta = currentPosition - lastPosition;
			lastPosition = currentPosition;
			transform.position += Vector3.left * diffDelta;
		}

		if(Input.GetMouseButtonUp(0) && Input.touchCount == 0){
			lastPosition = 0;
		}
	}
	
	public Direction MoveTo {
        get {
            return moveTo;
        }

        set {
            moveTo = value;
        }
    }

    public GameObject FollowTo {
        set {
            followTo = value;
        }
    }

    public float MinPosition {
        get {
            return minPosition;
        }
    }

    public float MaxPosition {
        get {
            return maxPosition;
        }
    }
}
