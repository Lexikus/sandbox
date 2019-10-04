using System;
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

	public Vec3 EulerAngles {
		get {
			float x, y, z, qx, qy, qz, qw, a2;

			qx = X;
			qy = Y;
			qz = Z;
			qw = W;

			a2 = 2 * (qw * qy - qx * qz);
			if (a2 <= -0.99999) {
				x = 2 * Mathf.Atan2(qx, qw);
				y = -Mathf.PI / 2;
				z = 0;
			} else if (a2 >= 0.99999) {
				x = 2 * Mathf.Atan2(qx, qw);
				y = Mathf.PI / 2;
				z = 0;
			} else {
				x = Mathf.Atan2(2 * (qw * qx + qy * qz), 1 - 2 * (qx * qx + qy * qy));
				y = Mathf.Asin(a2);
				z = Mathf.Atan2(2 * (qw * qz + qx * qy), 1 - 2 * (qy * qy + qz * qz));
			}

			return NormalizeAngles(new Vec3(x, y, z) * Mathf.Rad2Deg);
		}
	}

	public void Set(float x, float y, float z, float w) {
		X = x;
		Y = y;
		Z = z;
		W = w;
	}

	public void SetFromToRotation(Vec3 fromDirection, Vec3 toRotation) {
		throw new System.NotImplementedException();
	}

	public void SetLookRotation(Vec3 forward) {
		throw new System.NotImplementedException();
	}

	public void SetLookRotation(Vec3 forward, Vec3 upwards) {
		throw new System.NotImplementedException();
	}

	public void AngleAxis(out float angle, out Vec3 axis) {
		angle = 0;
		axis = Vec3.Up;
	}

	public static Quat Identity {
		get { return new Quat(0, 0, 0, 1); }
	}

	public static float Angle(Quat a, Quat b) {
		throw new System.NotImplementedException();
	}

	public static Quat AngleAxis(float angle, Vec3 axis) {
		throw new System.NotImplementedException();
	}

	public static float Dot(Quat q1, Quat q2) {
		return q1.X * q2.X + q1.Y * q2.Y + q1.Z * q2.Z + q1.W * q2.W;
	}

	public static Quat Euler(float x, float y, float z) {
		throw new System.NotImplementedException();
	}

	public static Quat Euler(Vec3 euler) {
		throw new System.NotImplementedException();
	}

	public static Quat FromToRotation(Vec3 fromDirection, Vec3 toRotation) {
		throw new System.NotImplementedException();
	}

	public static Quat Inverse(Quat rotation) {
		throw new System.NotImplementedException();
	}

	public static Quat LookRotation(Vec3 forward) {
		throw new System.NotImplementedException();
	}

	public static Quat LookRotation(Vec3 forward, Vec3 upwards) {
		throw new System.NotImplementedException();
	}

	public static Quat operator *(Quat left, Quat right) {
		float w = left.W * right.W - left.X * right.X - left.Y * right.Y - left.Z * right.Z;
		float x = left.W * right.X + left.X * right.W + left.Y * right.Z - left.Z * right.Y;
		float y = left.W * right.Y - left.X * right.Z + left.Y * right.W + left.Z * right.X;
		float z = left.W * right.Z + left.X * right.Y - left.Y * right.X + left.Z * right.W;
		return new Quat(x, y, z, w);
	}

	public override string ToString() {
		return "(" + X + ", " + Y + ", " + Z + ", " + W + ")";
	}

	private Vec3 NormalizeAngles(Vec3 angles) {
		return new Vec3(NormalizeAngle(angles.X), NormalizeAngle(angles.Y), NormalizeAngle(angles.Z));
	}

	private float NormalizeAngle(float angle) {
		while (angle > 360)
			angle -= 360;
		while (angle < 0)
			angle += 360;
		return angle;
	}
}