using UnityEngine;

public struct Vec3 {
    public float X { get; private set; }
    public float Y { get; private set; }
    public float Z { get; private set; }

    public Vec3(float x, float y, float z) {
        X = x;
        Y = y;
        Z = z;
    }

    public float Magnitude {
        get {
            return Mathf.Sqrt(Mathf.Pow(X, 2) + Mathf.Pow(Y, 2) + Mathf.Pow(Z, 2));
        }
    }

    public float SqrtMagnitude {
        get {
            return Mathf.Pow(X, 2) + Mathf.Pow(Y, 2) + Mathf.Pow(Z, 2);
        }
    }

    public Vec3 Normalized {
        get {
            float vM = Magnitude;
            return new Vec3(X / vM, Y / vM, Z / vM);
        }
    }

    public static Vec3 operator +(Vec3 left, Vec3 right) {
        return new Vec3(left.X + right.X, left.Y + right.Y, left.Z + right.Z);
    }

    public static Vec3 operator -(Vec3 left, Vec3 right) {
        return new Vec3(left.X - right.X, left.Y - right.Y, left.Z - right.Z);
    }

    public static float operator *(Vec3 left, Vec3 right) {
        return left.X * right.X + left.Y * right.Y + left.Z * right.Z; ;
    }

    public static Vec3 operator *(float t, Vec3 v) {
        return new Vec3(v.X * t, v.Y * t, v.Z * t);
    }

    public static Vec3 operator *(Vec3 v, float t) {
        return t * v;
    }

    public static Vec3 operator /(Vec3 v, float t) {
        return new Vec3(v.X / t, v.Y / t, v.Z / t);
    }

    public static bool operator ==(Vec3 left, Vec3 right) {
        if (!left.Equals(right)) return false;
        return true;
    }

    public static bool operator !=(Vec3 left, Vec3 right) {
        if (left.Equals(right)) return false;
        return true;
    }

    public static float Dot(Vec3 left, Vec3 right) {
        return left * right;
    }

    public static Vec3 Cross(Vec3 left, Vec3 right) {
        return new Vec3(
            left.Y * right.Z - left.Z * right.Y,
            left.Z * right.X - left.X * right.Z,
            left.X * right.Y - left.Y * right.X
        );
    }

    public override string ToString() {
        return "(" + X + ", " + Y + ", " + Z + ")";
    }

    public override bool Equals(object obj) {
        if (!(obj is Vec3)) return false;

        Vec3 other = (Vec3)obj;
        if (other.X != X) return false;
        if (other.Y != Y) return false;
        if (other.Z != Z) return false;
        return true;
    }

    public void Normalize() {
        float vM = Magnitude;
        X = X / vM;
        Y = Y / vM;
        Z = Z / vM;
    }

    public static Vec3 One {
        get {
            return new Vec3(1, 1, 1);
        }
    }

    public static Vec3 Left {
        get {
            return new Vec3(-1, 0, 0);
        }
    }

    public static Vec3 Right {
        get {
            return new Vec3(1, 0, 0);
        }
    }

    public static Vec3 Up {
        get {
            return new Vec3(0, 1, 0);
        }
    }

    public static Vec3 Down {
        get {
            return new Vec3(0, -1, 0);
        }
    }

    public static Vec3 Back {
        get {
            return new Vec3(0, 0, -1);
        }
    }

    public static Vec3 Forward {
        get {
            return new Vec3(0, 0, 1);
        }
    }

    public static Vec3 Zero {
        get {
            return new Vec3(0, 0, 0);
        }
    }

    public static float Angle(Vec3 left, Vec3 right) {
        float a = Mathf.Acos((left * right) / (right.Magnitude * left.Magnitude)) * Mathf.Rad2Deg;
        return a;
    }

    public void Scale(float f) {
        X *= f;
        Y *= f;
        Z *= f;
    }

    public static float Distance(Vec3 left, Vec3 right) {
        float x = right.X - left.X;
        float y = right.Y - left.Y;
        float z = right.Z - left.Z;
        return Mathf.Sqrt(Mathf.Pow(x, 2) + Mathf.Pow(y, 2) + Mathf.Pow(z, 2));
    }

    public static Vec3 Lerp(Vec3 left, Vec3 right, float t) {
        t = Mathf.Clamp(t, 0, 1);
        return (left + t * (right - left));
    }

    public static Vec3 LerpUnclamped(Vec3 left, Vec3 right, float t) {
        return (left + t * (right - left));
    }

    public static Vec3 MoveTowards(Vec3 left, Vec3 right, float maxDistanceDelta) {
        Vec3 a = right - left;
        float magnitude = a.Magnitude;
        if (magnitude <= maxDistanceDelta || magnitude == 0f) {
            return right;
        }
        return left + a / magnitude * maxDistanceDelta;
    }

    public Vector3 ToUnityVector3() {
        return new Vector3(X, Y, Z);
    }

    public void FromUnityVector3(Vector3 v) {
        X = v.x;
        Y = v.y;
        Z = v.z;
    }

    public static Vec3 CreateFromUnityVector3(Vector3 v) {
        return new Vec3(v.x, v.y, v.z);
    }

    public static Vector3[] ToVector3Array(Vec3[] v) {
        Vector3[] nv = new Vector3[v.Length];
        for (int i = 0; i < v.Length; i++) {
            nv[i] = v[i].ToUnityVector3();
        }
        return nv;
    }

    public static Vector2[] ToVector2Array(Vec3[] v) {
        Vector2[] nv = new Vector2[v.Length];
        for (int i = 0; i < v.Length; i++) {
            nv[i] = v[i].ToUnityVector3();
        }
        return nv;
    }

}
