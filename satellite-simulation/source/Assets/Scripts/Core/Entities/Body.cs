using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Trans))]
public class Body : MonoBehaviour {

    private static List<Trans> bodies = new List<Trans>();
    // so far, we'll support sphere colliders
    private SphereCollider _collider;

    void Start() {
        _collider = GetComponent<SphereCollider>();
        if (_collider == null) return;
        bodies.Add(GetComponent<Trans>());
    }

    private void FixedUpdate() {
        DetectCollision();
    }

    private void DetectCollision() {
        foreach (var check in bodies.ToArray()) {
            SphereCollider checkCollider = GetComponent<SphereCollider>();
            foreach (var with in bodies.ToArray()) {
                if (check == with) continue;
                SphereCollider withCollider = GetComponent<SphereCollider>();
                if (Vec3.Distance(check.Position, with.Position) <= withCollider.radius + checkCollider.radius) {
                    DestroyBody(check);
                    DestroyBody(with);
                }
            }
        }
    }

    private void DestroyBody(Trans body) {
        if (body.CompareTag("Earth")) return;
        bodies.Remove(body);
        Destroy(body.gameObject);
    }

    public static int BodyCounts() {
        return bodies.Count;
    }
}
