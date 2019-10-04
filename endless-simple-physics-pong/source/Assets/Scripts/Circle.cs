using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Circle : MonoBehaviour {

	public Trans _transform { get; private set; }

	[SerializeField] private float radius;

	public float Radius { get { return radius; } }

	private void Start() {
		_transform = GetComponent<Trans>();
	}
}
