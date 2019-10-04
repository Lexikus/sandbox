using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthController : MonoBehaviour {
    private Trans _transform;

    private void Start() {
        _transform = gameObject.GetComponent<Trans>();
    }

    // Update is called once per frame
    void Update() {
        _transform.RotateY(Time.deltaTime);
    }
}
