using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenSatelliteController : MonoBehaviour {

    [SerializeField] float speed = 1;
    [SerializeField] private GameObject earthGameObject;
    private Vector3 earth;
    private float earthMass = 10;

    private GameObject satelliteGameObject;
    private Vector3 satellite;
    private float satelliteMass = 2;

    private float radius;
    private Vector3 radiusDir;

    private float force;
    private Vector3 forceDir;

    private float velocity;
    private Vector3 velocityDir;

    private float acceleraton;
    private Vector3 acceleratonDir;

    // this is the eccentricity of the earth
    //private float eccentricity = 0.0167f;
    private float eccentricity = 0.0167f;
    private static float semiMajorAxis;
    private static float semiMinorAxis;

    private float orbitalPeriod;
    private float t = 0;

    // 6.67408E-10f would be the real Gravity constant but for our simulation it's to small
    //private const float G = 6.67408E-10f;
    private const float G = 1f;

    private LineRenderer lineRenderer;

    // Use this for initialization
    void Start() {
        earth = earthGameObject.transform.position;
        satelliteGameObject = gameObject;
        satellite = satelliteGameObject.transform.position;
        lineRenderer = gameObject.AddComponent<LineRenderer>();

        CalculateRadius();
        CalculateSemiAxis();
        CalculateOrbitalPeriod();

        DebugDrawOrbit();
    }

    private void CalculateOrbitalPeriod() {
        // good to know but we don't need it really
        orbitalPeriod = 2 * Mathf.PI * Mathf.Sqrt(Mathf.Pow(semiMajorAxis, 3) / (G * satelliteMass));
    }

    private void CalculateSemiAxis() {
        // let's assume the semiMajorAxis is the double distance from the earth to the satellite
        // so the first satellite should crash - I guess :/
        // we need this to draw the orbit.
        if (semiMajorAxis != 0) return;
        semiMajorAxis = radius * 2;
        semiMinorAxis = semiMajorAxis * Mathf.Sqrt(1 - Mathf.Pow(eccentricity, 2));
    }

    private void CalculateRadius() {
        // simple vector direction calculation
        Vector3 distance = earth - satellite;
        radiusDir = distance.normalized;
        radius = distance.magnitude;
    }

    private Vector3 GetPointOnEllipse(float t) {
        // might be helpfull to draw the ellipse if needed
        return new Vector3(semiMajorAxis * Mathf.Cos(t * (Mathf.PI * 2)), 0, semiMinorAxis * Mathf.Sin(t * (Mathf.PI * 2)));
    }

    private void CalculateForce() {
        // force calculation of two bodies
        force = -(G * earthMass * satelliteMass / Mathf.Pow(radius, 2));

        // the force acts on satellitemass and the radius direction is from the satellite to the earth,
        // so the force must be inverted
        forceDir = -force * radiusDir;
    }

    private void CalculateVelocity() {
        // we get faster when we're closer to the earth
        velocity = Mathf.Sqrt(2 * G * satelliteMass / radius);

        float m = -Mathf.Pow(semiMajorAxis, 2) * satellite.x / Mathf.Pow(semiMinorAxis, 2) * satellite.y;
        velocityDir = new Vector3(m, 1, 0).normalized * velocity;
    }

    private void CalculateAcceleration() {
        // there is an acceleration of the satellite mass to the earth.
        acceleraton = -Mathf.Sqrt((G * earthMass / Mathf.Pow(radius, 2)));
        // the radius vector looks from the satellite to earth, so we need to invert it.
        acceleratonDir = -acceleraton * radiusDir;
    }

    private void CalculatePeriod() {
        t += Time.deltaTime * speed;
        if (t >= 1) {
            t = 0;
        }
    }

    private void DebugDrawOrbit() {
        lineRenderer.positionCount = 100;
        lineRenderer.widthMultiplier = 0.05f;
        for (int i = 0; i < 100; i++) {
            lineRenderer.SetPosition(i, GetPointOnEllipse(i / 100f));
        }
    }

    private void FixedUpdate() {
        CalculatePeriod();
        CalculateRadius();
        CalculateForce();
        CalculateVelocity();
        CalculateAcceleration();

        Vector3 angularMomentun = Vector3.Cross(radiusDir * radius, velocityDir);

        Debug.DrawLine(transform.position, transform.position + velocityDir);
        Debug.DrawLine(transform.position, angularMomentun, Color.blue);
        //Debug.DrawLine(transform.position, transform.position + radius * radiusDir, Color.red);
        //Debug.DrawLine(transform.position, transform.position + forceDir, Color.cyan);
        //Debug.DrawLine(transform.position, transform.position + acceleratonDir, Color.magenta);

        satelliteGameObject.transform.Translate(angularMomentun * speed * Time.fixedDeltaTime);
        //satelliteGameObject.transform.Translate(forceDir * speed * Time.fixedDeltaTime);

        // keep the vectors updated
        satellite = satelliteGameObject.transform.position;
        earth = earthGameObject.transform.position;
    }

    private void FakeMovement() {
        // the satellite is on the ellipse perfectly
        // and it works only 2D
        satellite = GetPointOnEllipse(t);
        satelliteGameObject.transform.position = satellite;
    }
}
