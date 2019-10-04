using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Controller : MonoBehaviour {
    [SerializeField] private float speed = 1;
    [SerializeField] private GameObject satellitePrefab;

    private Vec3 direction;

    private void Start() {
        Time.timeScale = speed;
        direction = Vec3.Right;
    }

    public void ChangeSpeed(float value) {
        Time.timeScale = value;
    }

    public void ChangeDirection(string _direction) {
        switch (_direction.ToLower()) {
            case "u": direction = Vec3.Up; break;
            case "r": direction = Vec3.Right; break;
            case "d": direction = Vec3.Down; break;
            case "l": direction = Vec3.Left; break;
        }
        SpawnSatellite();
    }

    private void SpawnSatellite() {
        Vec3 startPos = Vec3.CreateFromUnityVector3(Camera.main.transform.position);
        startPos = new Vec3(startPos.X, startPos.Y, startPos.Z + 10);
        GameObject g = Instantiate(satellitePrefab, startPos.ToUnityVector3(), Quaternion.identity);
        g.GetComponent<SatelliteController>().Init(direction);
    }
}
