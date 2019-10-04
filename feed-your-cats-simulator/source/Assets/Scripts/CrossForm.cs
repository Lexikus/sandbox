using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossForm : Form {
    /*
    0 1 0
    1 1 1
    0 1 0
    */
    public const int WIDTH = 3;
    public const int HEIGHT = 3;

    public override int Width {
        get { return WIDTH; }
    }

    public override int Height {
        get { return HEIGHT; }
    }

    private readonly int[] VALID_CONNECTORS_SLOT = { 1, 3, 5, 7 };

    private void Awake() {
        formMatrix = new int[WIDTH * HEIGHT];
        connectorMatrix = new int[WIDTH * HEIGHT];

        formMatrix[1] = 1;
        formMatrix[3] = 1;
        formMatrix[4] = 1;
        formMatrix[5] = 1;
        formMatrix[7] = 1;

        AssignConnectorSlots();
    }

    private void AssignConnectorSlots() {
        foreach (var item in VALID_CONNECTORS_SLOT) {
            int slot = Random.Range(1, VALID_CONNECTORS_SLOT.Length);
            connectorMatrix[slot] = 1;
        }
    }
}
