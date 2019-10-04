using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// there'll never be a perfect orbit because of the floating points. sorry
public class UnitySatelliteController : MonoBehaviour {
    [SerializeField] private GameObject earthGameObject;
    private Vector3 earth;

    //the real mass of the earth is 5.972E+24
    private double earthMass = 5.972E+14;

    private GameObject satelliteGameObject;
    private Vector3 satellite;
    private double satelliteMass = 1000;

    private double velocity;
    private Vector3 velocityDir;

    private double acceleration;
    private Vector3 accelerationDir;

    private double radius;
    private Vector3 radiusDir;

    private const double G = 6.67408E-11;
    private float deltaTime;

    void Start() {
        satelliteGameObject = gameObject;
        earth = earthGameObject.transform.position;
        deltaTime = Time.deltaTime;

        // let's give our satellite a initial velocity and direction.
        velocity = 25;
        velocityDir = Vector3.right * (float)velocity;

        UpdateVectors();
        CalculateRadius();
    }

    // this calculates the radius from satellite to earth.
    // keep in mind, it saves only the normalized vector. The length is saved in radius itself
    // it's important for all other vectors
    private void CalculateRadius() {
        Vector3 distance = satellite - earth;
        radius = distance.magnitude;
        radiusDir = distance.normalized;
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
        satelliteGameObject.transform.Translate(velocityDir * deltaTime);

        // it's helpful to see the orbit
        //GameObject.CreatePrimitive(UnityEngine.PrimitiveType.Sphere).gameObject.transform.position = satelliteGameObject.transform.position;
    }

    private void UpdateVectors() {
        satellite = satelliteGameObject.transform.position;
        earth = earthGameObject.transform.position;
    }

    private void FixedUpdate() {
        CalculateRadius();
        CalculateAcceleration();
        CalculateVelocity();
        ActForcesOnSatellite();
        UpdateVectors();
        deltaTime = Time.fixedDeltaTime;
    }
}
