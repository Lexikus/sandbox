using UnityEngine;

public struct Vec4 {
    public float X { get; private set; }
    public float Y { get; private set; }
    public float Z { get; private set; }
    public float W { get; private set; }

    public Vec4(float x, float y, float z, float w) {
        X = x;
        Y = y;
        Z = z;
        W = w;
    }

    public static Vec4 CreateFromUnityVector3(Vector4 v) {
        return new Vec4(v.x, v.y, v.z, v.w);
    }
}