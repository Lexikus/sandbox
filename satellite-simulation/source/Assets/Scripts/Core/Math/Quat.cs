using UnityEngine;

public struct Quat {
    public float X { get; private set; }
    public float Y { get; private set; }
    public float Z { get; private set; }
    public float W { get; private set; }

    public Quat(float x, float y, float z, float w) {
        X = x;
        Y = y;
        Z = z;
        W = w;
    }

    public static Quat CreateFromUnityQuaternion(Quaternion v) {
        return new Quat(v.x, v.y, v.z, v.w);
    }

    public Quaternion ToUnityQuaternion() {
        return new Quaternion(X, Y, Z, W);
    }
}