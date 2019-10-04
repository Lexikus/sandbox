using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plane : Primitive {

    private void Start() {
        Type = PrimitiveType.Plane;
        Mesh mesh = Primitives.Plane(10, 10);
        filter.mesh = mesh;
    }
}
