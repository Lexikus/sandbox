using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(GameController), true)]
public class PlayerSpawnEditor : Editor {
    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        GameController gameController = (GameController)target;

        if (GUILayout.Button("Spawn Player")) {
            gameController.SpawnPlayer();
        }

        if (GUILayout.Button("Spawn Enemy")) {
            gameController.SpawnEnemies();
        }
    }
}