using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    private Trans _transform;

    float vertical = 0;
    float horizontal = 0;

    void Start() {
        _transform = GetComponent<Trans>();
        // our transform can't initialize rotation yet.
        _transform.RotateX(21);
    }

    void Update() {
        SimpleCameraMovement();
    }

    private void SimpleCameraMovement() {
        vertical = 0;
        horizontal = 0;

        if (Input.GetButton("Horizontal")) horizontal = Input.GetAxis("Horizontal");
        if (Input.GetButton("Vertical")) vertical = Input.GetAxis("Vertical");

        Vec3 dir = new Vec3(horizontal, 0, vertical);
        _transform.Translate(dir);
    }
}
