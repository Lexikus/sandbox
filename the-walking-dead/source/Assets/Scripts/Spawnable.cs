using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawnable : MonoBehaviour {
    public static Spawnable Instance { get; private set; }

    [SerializeField] private Transform[] spawnables;
    private List<int> takenSlots = new List<int>();

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        } else if (Instance != this) {
            Destroy(this);
        }
    }

    public bool HasSlots() {
        return takenSlots.Count < spawnables.Length;
    }

    public Vector3 GetSpawnablePosition() {
        int length = spawnables.Length;
        int slot = Random.Range(0, length);

        if (takenSlots.Contains(slot)) slot = GetFreeSlot();

        takenSlots.Add(slot);
        return spawnables[slot].position;
    }

    private int GetFreeSlot() {
        int freeSlot = 0;
        for (int i = 0; i < spawnables.Length; i++) {
            freeSlot = i;
            if (!takenSlots.Contains(i)) break;
        }

        return freeSlot;
    }
}
