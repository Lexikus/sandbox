using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnZombies : MonoBehaviour {

    [SerializeField] private GameObject zombiePrefab;

    [SerializeField] private int amountToSpawn = 5;

    void Start() {
        for (int i = 0; i < amountToSpawn; i++) {
            if (!Spawnable.Instance.HasSlots()) break;

            Vector3 position = Spawnable.Instance.GetSpawnablePosition();
            GameObject spawnedObject = GameObject.Instantiate(zombiePrefab, position, Quaternion.identity);
            spawnedObject.transform.SetParent(transform);
        }
    }
}
