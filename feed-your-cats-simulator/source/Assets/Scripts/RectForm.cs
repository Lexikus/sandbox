using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RectForm : Form {
    /*
    1 1 1
    1 1 1
    */
    public const int WIDTH = 3;
    public const int HEIGHT = 2;

    public override int Width {
        get { return WIDTH; }
    }

    public override int Height {
        get { return HEIGHT; }
    }

    private readonly int[] VALID_CONNECTORS_SLOT = { 0, 2, 4 };

    private void Awake() {
        formMatrix = new int[WIDTH * HEIGHT];
        connectorMatrix = new int[WIDTH * HEIGHT];

        for (int i = 0; i < WIDTH * HEIGHT; i++) {
            formMatrix[i] = 1;
        }

        AssignConnectorSlots();
    }

    private void AssignConnectorSlots() {
        foreach (var item in VALID_CONNECTORS_SLOT) {
            int slot = Random.Range(1, VALID_CONNECTORS_SLOT.Length);
            connectorMatrix[slot] = 1;
        }
    }
}
