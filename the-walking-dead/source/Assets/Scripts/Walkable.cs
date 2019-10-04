using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walkable : MonoBehaviour {
    public static Walkable Instance { get; private set; }

    [SerializeField] private Transform[] walkables;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        } else if (Instance != this) {
            Destroy(this);
        }
    }

    public Vector3 GetWalkablePosition() {
        int length = walkables.Length;
        return walkables[Random.Range(0, length)].position;
    }
}
