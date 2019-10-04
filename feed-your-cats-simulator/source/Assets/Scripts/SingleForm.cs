using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleForm : Form {
	/*
    1
    */
	public const int WIDTH = 1;
	public const int HEIGHT = 1;

	public override int Width {
		get { return WIDTH; }
	}

	public override int Height {
		get { return HEIGHT; }
	}

	private void Awake() {
		formMatrix = new int[WIDTH * HEIGHT];
		connectorMatrix = new int[WIDTH * HEIGHT];

		for (int i = 0; i < WIDTH * HEIGHT; i++) {
			formMatrix[i] = 1;
			connectorMatrix[i] = 1;
		}
	}
}
