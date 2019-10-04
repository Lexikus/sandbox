using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PrimitiveType {
    None,
    Plane,
    Sphere,
    Cube,
}

public abstract class Primitive : MonoBehaviour {
    private Vec3 position;
    private Vec3 rotation;

    public PrimitiveType Type { get; protected set; }

    protected MeshRenderer renderer = null;
    protected MeshFilter filter = null;

    public Vec3 Position {
        get { return position; }
        set {
            position = value;
            transform.position = value.ToUnityVector3();
        }
    }

    public Vec3 Rotation {
        get { return rotation; }
        set {
            rotation = value;
            transform.rotation = Quaternion.Euler(value.ToUnityVector3());
        }
    }

    protected virtual void Awake() {
        position = Vec3.CreateFromUnityVector3(transform.position);
        rotation = Vec3.CreateFromUnityVector3(transform.rotation.eulerAngles);
        renderer = gameObject.AddComponent<MeshRenderer>();
        renderer.material = new Material(Shader.Find("Diffuse"));
        filter = gameObject.AddComponent<MeshFilter>();
    }
}
