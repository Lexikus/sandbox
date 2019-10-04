using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using InputKey = CharacterInput.InputKey;

public class CharacterShoot : MonoBehaviour {
    [SerializeField] private GameObject projectilePrefab;

    public void HandleProjectile(InputKey input) {
        if (input == InputKey.ShootUp) Shoot(transform.position, 0);
        else if (input == InputKey.ShootRight) Shoot(transform.position, -90);
        else if (input == InputKey.ShootDown) Shoot(transform.position, -180);
        else if (input == InputKey.ShootLeft) Shoot(transform.position, -270);
        else if (input == InputKey.ShootUpperLeft) Shoot(transform.position, -315);
        else if (input == InputKey.ShootUpperRight) Shoot(transform.position, -45);
        else if (input == InputKey.ShootDownLeft) Shoot(transform.position, -225);
        else if (input == InputKey.ShootDownRight) Shoot(transform.position, -135);
    }

    private GameObject SpawnProjectile(Vector2 position, Quaternion rotation) {
        return Instantiate(projectilePrefab, position, rotation);
    }

    private void Shoot(Vector2 position, float degreeRotation) {
        SpawnProjectile(position + OffsetFromDegree(degreeRotation), Quaternion.AngleAxis(degreeRotation, Vector3.forward));
    }

    private Vector2 OffsetFromDegree(float degree) {
        if (degree == -90) return Vector2.right;
        else if (degree == -180) return Vector2.down;
        else if (degree == -270) return Vector2.left;
        else if (degree == -315) return Vector2.left + Vector2.up;
        else if (degree == -45) return Vector2.right + Vector2.up;
        else if (degree == -225) return Vector2.left + Vector2.down;
        else if (degree == -135) return Vector2.right + Vector2.down;
        return Vector2.up;
    }
}
