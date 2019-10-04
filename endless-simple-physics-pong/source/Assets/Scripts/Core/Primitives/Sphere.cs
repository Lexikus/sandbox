using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sphere : Primitive {
	[SerializeField] private int _long;
	[SerializeField] private int lat;
	[SerializeField] private float radius;

	protected override void Awake() {
		base.Awake();
		Type = PrimitiveType.Sphere;
	}

	private void Start() {
		Mesh mesh = Primitives.Sphere(radius, _long, lat);
		meshFilter.mesh = mesh;
	}
}
