using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnpointManager : MonoBehaviour {

    public static EnemySpawnpointManager Instance { get; private set; }
    private List<Vector2> spawnpoints;

    private void Awake() {
        if (Instance == null) Instance = this;
        else if (Instance != this) Destroy(this);
    }

    private void Start() {
        spawnpoints = new List<Vector2>();

        GetSpawnpointsInChildren();
    }

    private void GetSpawnpointsInChildren() {
        foreach (Transform item in transform) {
            spawnpoints.Add(item.transform.position);
        }
    }

    public Vector2 GetRandomSpawnpoint() {
        if (spawnpoints.Count == 0) return Vector2.zero;
        return spawnpoints[Random.Range(0, spawnpoints.Count)];
    }
}
