using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawnpointManager : MonoBehaviour {

    public static PlayerSpawnpointManager Instance { get; private set; }
    private Dictionary<int, Vector2> spawnpoints;

    private void Awake() {
        if (Instance == null) Instance = this;
        else if (Instance != this) Destroy(this);
    }

    private void Start() {
        spawnpoints = new Dictionary<int, Vector2>();

        GetSpawnpointsInChildren();
    }

    private void GetSpawnpointsInChildren() {
        foreach (Transform item in transform) {
            spawnpoints.Add(spawnpoints.Count, item.transform.position);
        }
    }

    public Vector2 GetSpawnpoint(int id) {
        Vector2 position = Vector2.zero;

        id = id - 1;
        if (spawnpoints.ContainsKey(id)) position = spawnpoints[id];

        return position;
    }
}
