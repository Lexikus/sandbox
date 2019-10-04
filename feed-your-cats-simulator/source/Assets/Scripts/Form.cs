using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Form : MonoBehaviour {
    protected int[] formMatrix;
    protected int[] connectorMatrix;

    public abstract int Width { get; }
    public abstract int Height { get; }


    public int[] FormMatrix {
        get {
            return formMatrix;
        }
    }

    public int[] ConnectorMatrix {
        get {
            return connectorMatrix;
        }
    }

    public int ValidConnectorsCount() {
        int i = 0;
        foreach (var item in connectorMatrix) {
            if (item == 0) continue;
            i++;
        }
        return i;
    }
}
