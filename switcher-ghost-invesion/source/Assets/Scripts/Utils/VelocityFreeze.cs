using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VelocityFreeze : MonoBehaviour {

    private Rigidbody2D _rb;

    private void Start() {
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    private void FixedUpdate() {
        if (_rb == null) return;
        if (_rb.velocity == Vector2.zero) return;

        _rb.velocity = Vector2.zero;
    }
}
