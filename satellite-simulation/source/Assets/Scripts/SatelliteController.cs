using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// there'll never be a perfect orbit because of the floating points. sorry
public class SatelliteController : MonoBehaviour {
    private GameObject earthGameObject;
    private Vec3 earth;

    //the real mass of the earth is 5.972E+24
    private double earthMass = 5.972E+14;

    private GameObject satelliteGameObject;
    private Vec3 satellite;
    private double satelliteMass = 1000;

    private double velocity;
    private Vec3 velocityDir;

    private double acceleration;
    private Vec3 accelerationDir;

    private double radius;
    private Vec3 radiusDir;

    private const double G = 6.67408E-11;
    private float deltaTime;

    private Trans _transform;

    private bool isReady = false;

    Dictionary<string, string> informations = new Dictionary<string, string>();

    public void Init(Vec3 dir, double v = 25f) {
        satelliteGameObject = gameObject;
        _transform = satelliteGameObject.GetComponent<Trans>();
        deltaTime = Time.deltaTime;
        earthGameObject = GameObject.FindGameObjectWithTag("Earth");

        velocity = v;
        velocityDir = dir.Normalized * (float)velocity;

        UpdateVectors();
        CalculateRadius();
        isReady = true;
    }

    public void Init(Vector3 dir, double v = 25) {
        Init(Vec3.CreateFromUnityVector3(dir), v);
    }

    private void UpdateInformations() {
        informations["satellitemass"] = satelliteMass.ToString();
        informations["velocity"] = velocity.ToString();
        informations["acceleration"] = acceleration.ToString();
        informations["radius"] = radius.ToString();
    }

    public Dictionary<string, string> GetInformations() {
        return informations;
    }

    // this calculates the radius from satellite to earth.
    // keep in mind, it saves only the normalized vector. The length is saved in radius itself
    // it's important for all other vectors
    private void CalculateRadius() {
        Vec3 distance = satellite - earth;
        radius = distance.Magnitude;
        radiusDir = distance.Normalized;
    }

    // this calculates the velocity and direction. We need to know where it was before to calculate the direction
    // https://en.wikipedia.org/wiki/Newton%27s_laws_of_motion#Newton's_second_law
    // https://en.wikipedia.org/wiki/Escape_velocity
    private void CalculateVelocity() {
        velocity = Math.Sqrt(G * earthMass / radius);
        velocityDir += accelerationDir * deltaTime;
    }

    // Because the earth mass is larger than the satellite, let's define an attractive gravitational field.
    // https://en.wikipedia.org/wiki/Gravitational_acceleration
    private void CalculateAcceleration() {
        acceleration = -G * earthMass / Math.Pow(radius, 2);
        accelerationDir = (float)acceleration * radiusDir;
    }

    // the satellite will crash on earth directly without any direction velocity.
    // also there is a force which acts on the satellite from earth and of the opposite.
    // they cancel each other tho.
    private void ActForcesOnSatellite() {
        _transform.Translate(velocityDir * deltaTime);

        // it's helpful to see the orbit
        //GameObject.CreatePrimitive(UnityEngine.PrimitiveType.Sphere).gameObject.transform.position = satelliteGameObject.transform.position;
    }

    private void UpdateVectors() {
        satellite = _transform.Position;
        earth = earthGameObject.GetComponent<Trans>().Position;
    }

    private void FixedUpdate() {
        if (!isReady) return;
        CalculateRadius();
        CalculateAcceleration();
        CalculateVelocity();
        ActForcesOnSatellite();
        UpdateVectors();
        UpdateInformations();
        deltaTime = Time.fixedDeltaTime;
    }
}
