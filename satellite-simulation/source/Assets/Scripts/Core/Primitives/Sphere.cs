using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sphere : Primitive {
    public int Long { get; private set; }
    public int Lat { get; private set; }
    public float Radius { get; private set; }

    protected override void Awake() {
        base.Awake();
        Type = PrimitiveType.Sphere;
        Long = 10;
        Lat = 10;
        Radius = 1;
    }

    private void Start() {
        Mesh mesh = Primitives.Sphere(Radius, Long, Lat);
        filter.mesh = mesh;
    }
}
