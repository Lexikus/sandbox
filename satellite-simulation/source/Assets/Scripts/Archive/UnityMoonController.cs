using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityMoonController : MonoBehaviour {
    [SerializeField] private GameObject earth;

    private float earthMoonDistance = 0;

    private float semiMajorAxis = 0;
    private float semiMinorAxis = 0;

    // this is the eccentricity of the moon
    // Didn't bother to calculate it by my self.
    [SerializeField] private float eccentricity = 0.0549f;

    // orbital period
    private float t = 0;
    private float speed = 0.1f;

    private void Start() {
        CalculateEarthMoonDistance();
        CalculateSemiAxis();
    }

    private void FixedUpdate() {
        CalculateEarthMoonDistance();
        Period();
        RotateMoon();
    }

    private void RotateMoon() {
        // Rotate the moon with the information given from the ellipse
        transform.position = (new Vector3(semiMajorAxis * Mathf.Cos(t * (Mathf.PI * 2)), 0, semiMinorAxis * Mathf.Sin(t * (Mathf.PI * 2))));
    }

    private void Period() {
        t += Time.fixedDeltaTime * speed;
        if (t >= 1) {
            t = 0;
        }
    }

    private void CalculateSemiAxis() {
        // let's assume the semiMajorAxis is the distance from the earth
        semiMajorAxis = earthMoonDistance;
        semiMinorAxis = semiMajorAxis * Mathf.Sqrt(1 - Mathf.Pow(eccentricity, 2));
    }

    // calculates the distance between the earth and moon
    private void CalculateEarthMoonDistance() {
        earthMoonDistance = Vector3.Distance(earth.transform.position, transform.position);
    }
}
