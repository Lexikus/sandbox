// Documentation is for me, primarly

using System;
using System.Collections.Generic;
using UnityEngine;

// Always use the properties even in the class itself 'cause it is connected with the Unity transform.

public class Trans : MonoBehaviour {
	private Vec3 position;
	public Vec3 Position {
		get { return position; }
		set {
			position = value;
			transform.position = position.ToUnityVector3();
		}
	}
	private Quat rotation;
	public Quat Rotation {
		get { return rotation; }
		set {
			rotation = value;
			transform.rotation = rotation.ToUnityQuaternion();
		}
	}
	private Vec3 localScale;
	public Vec3 LocalScale {
		get { return localScale; }
		set {
			localScale = value;
			transform.localScale = localScale.ToUnityVector3();
		}
	}

	// it's 4x4 column major matrix. Wikipedia articles are ROW MAJOR!.
	private float[] matrix = new float[4 * 4];

	// We have to get the Unity's Vectors to simulate our Transform.
	private void Awake() {
		position = Vec3.CreateFromUnityVector3(transform.position);
		rotation = Quat.CreateFromUnityQuaternion(transform.rotation);
		localScale = Vec3.CreateFromUnityVector3(transform.localScale);
		ResetMatrix();
	}

	public void ResetMatrix() {
		IdentifyMatrix();
		CreateMatrixFromPositionRotationScale();
	}

	private void IdentifyMatrix() {
		//1 0 0 0
		//0 1 0 0
		//0 0 1 0
		//0 0 0 1
		matrix[0] = 1;
		matrix[1] = 0;
		matrix[2] = 0;
		matrix[3] = 0;
		matrix[4] = 0;
		matrix[5] = 1;
		matrix[6] = 0;
		matrix[7] = 0;
		matrix[8] = 0;
		matrix[9] = 0;
		matrix[10] = 1;
		matrix[11] = 0;
		matrix[12] = 0;
		matrix[13] = 0;
		matrix[14] = 0;
		matrix[15] = 1;
	}

	// we'll asume that the rotation is 0,0,0 at the beginning.
	private void CreateMatrixFromPositionRotationScale() {
		// set current position into matrix.
		// https://en.wikipedia.org/wiki/Translation_(geometry)
		// keep in mind, wikipedia articles are row major

		// set current scale into matrix
		// https://en.wikipedia.org/wiki/Scaling_(geometry)
		matrix[0] = localScale.X;
		matrix[5] = localScale.Y;
		matrix[10] = localScale.Z;

		matrix[12] = position.X;
		matrix[13] = position.Y;
		matrix[14] = position.Z;
	}

	public void Translate(Vec3 translation) {
		// https://en.wikipedia.org/wiki/Translation_(geometry)
		// Again, Unity is column major and wikipedia articles are row major. We have to invert the matrix.
		float[] m = new float[4 * 4];
		m[0] = 1; m[1] = 0; m[2] = 0; m[3] = 0;
		m[4] = 0; m[5] = 1; m[6] = 0; m[7] = 0;
		m[8] = 0; m[9] = 0; m[10] = 1; m[11] = 0;
		m[12] = translation.X; m[13] = translation.Y; m[14] = translation.Z; m[15] = 1;

		MultiplyWith4x4Matrix(m);

		Position = new Vec3(matrix[12], matrix[13], matrix[14]);
	}

	// we use degrees
	public void RotateX(float angle) {
		angle = Mathf.Deg2Rad * angle;

		// https://en.wikipedia.org/wiki/Rotation_matrix
		// Again, Unity is column major and wikipedia articles are row major. We have to invert the matrix.

		float[] m = new float[4 * 4];
		m[0] = 1; m[1] = 0; m[2] = 0; m[3] = 0;
		m[4] = 0; m[5] = Mathf.Cos(angle); m[6] = Mathf.Sin(angle); m[7] = 0;
		m[8] = 0; m[9] = -Mathf.Sin(angle); m[10] = Mathf.Cos(angle); m[11] = 0;
		m[12] = 0; m[13] = 0; m[14] = 0; m[15] = 1;

		MultiplyWith4x4Matrix(m);

		Rotation = GetRotationFromMatrix();
	}

	public void RotateY(float angle) {
		angle = Mathf.Deg2Rad * angle;

		// https://en.wikipedia.org/wiki/Rotation_matrix
		// Again, Unity is column major and wikipedia articles are row major. We have to invert the matrix.

		float[] m = new float[4 * 4];
		m[0] = Mathf.Cos(angle); m[1] = 0; m[2] = -Mathf.Sin(angle); m[3] = 0;
		m[4] = 0; m[5] = 1; m[6] = 0; m[7] = 0;
		m[8] = Mathf.Sin(angle); m[9] = 0; m[10] = Mathf.Cos(angle); m[11] = 0;
		m[12] = 0; m[13] = 0; m[14] = 0; m[15] = 1;

		MultiplyWith4x4Matrix(m);

		Rotation = GetRotationFromMatrix();
	}

	public void RotateZ(float angle) {
		angle = Mathf.Deg2Rad * angle;

		// https://en.wikipedia.org/wiki/Rotation_matrix
		// Again, Unity is column major and wikipedia articles are row major. We have to invert the matrix.

		float[] m = new float[4 * 4];
		m[0] = Mathf.Cos(angle); m[1] = Mathf.Sin(angle); m[2] = 0; m[3] = 0;
		m[4] = -Mathf.Sin(angle); m[5] = Mathf.Cos(angle); m[6] = 0; m[7] = 0;
		m[8] = 0; m[9] = 0; m[10] = 1; m[11] = 0;
		m[12] = 0; m[13] = 0; m[14] = 0; m[15] = 1;

		MultiplyWith4x4Matrix(m);

		Rotation = GetRotationFromMatrix();
	}

	public void Scale(Vec3 scale) {
		// https://en.wikipedia.org/wiki/Scaling_(geometry)
		// Again, Unity is column major and wikipedia articles are row major. We have to invert the matrix.

		float[] m = new float[4 * 4];
		m[0] = scale.X; m[1] = 0; m[2] = 0; m[3] = 0;
		m[4] = 0; m[5] = scale.Y; m[6] = 0; m[7] = 0;
		m[8] = 0; m[9] = 0; m[10] = scale.Z; m[11] = 0;
		m[12] = 0; m[13] = 0; m[14] = 0; m[15] = 1;

		MultiplyWith4x4Matrix(m);

		LocalScale = new Vec3(
			Mathf.Sqrt(Mathf.Pow(matrix[0], 2) + Mathf.Pow(matrix[1], 2) + Mathf.Pow(matrix[2], 2)),
			Mathf.Sqrt(Mathf.Pow(matrix[4], 2) + Mathf.Pow(matrix[5], 2) + Mathf.Pow(matrix[6], 2)),
			Mathf.Sqrt(Mathf.Pow(matrix[8], 2) + Mathf.Pow(matrix[9], 2) + Mathf.Pow(matrix[10], 2))
		);
	}


	private Quat GetRotationFromMatrix() {
		float trace = matrix[0] + matrix[5] + matrix[10];
		float s = 0;

		Quat q;
		float x;
		float y;
		float z;
		float w;

		if (trace > 0) {
			s = Mathf.Sqrt(trace + 1.0f) * 2;
			x = (matrix[6] - matrix[9]) / s;

			y = (matrix[8] - matrix[2]) / s;
			z = (matrix[1] - matrix[4]) / s;

			w = 0.25f * s;
			q = new Quat(x, y, z, w);
		} else if ((matrix[0] > matrix[5]) && (matrix[0] > matrix[10])) {
			s = Mathf.Sqrt(1.0f + matrix[0] - matrix[5] - matrix[10]) * 2;
			x = 0.25f * s;

			y = (matrix[1] + matrix[4]) / s;
			z = (matrix[8] + matrix[2]) / s;

			w = (matrix[6] - matrix[9]) / s;
			q = new Quat(x, y, z, w);
		} else if (matrix[5] > matrix[10]) {
			s = Mathf.Sqrt(1.0f + matrix[5] - matrix[0] - matrix[10]) * 2;
			x = (matrix[1] + matrix[4]) / s;

			y = 0.25f * s;
			z = (matrix[6] + matrix[9]) / s;

			w = (matrix[8] - matrix[2]) / s;
			q = new Quat(x, y, z, w);
		} else {
			s = Mathf.Sqrt(1.0f + matrix[10] - matrix[0] - matrix[5]) * 2;
			x = (matrix[8] + matrix[2]) / s;

			y = (matrix[6] + matrix[9]) / s;
			z = 0.25f * s;

			w = (matrix[1] - matrix[4]) / s;
			q = new Quat(x, y, z, w);
		}
		return q;
	}

	private void MultiplyWith4x4Matrix(float[] mult) {
		if (mult.Length != 16) throw new Exception("mult: It has to be a 4x4 matrix");

		matrix[0] = matrix[0] * mult[0] + matrix[4] * mult[1] + matrix[8] * mult[2] + matrix[12] * mult[3];
		matrix[1] = matrix[1] * mult[0] + matrix[5] * mult[1] + matrix[9] * mult[2] + matrix[13] * mult[3];
		matrix[2] = matrix[2] * mult[0] + matrix[6] * mult[1] + matrix[10] * mult[2] + matrix[14] * mult[3];
		matrix[3] = matrix[3] * mult[0] + matrix[7] * mult[1] + matrix[11] * mult[2] + matrix[15] * mult[3];

		matrix[4] = matrix[0] * mult[4] + matrix[4] * mult[5] + matrix[8] * mult[6] + matrix[12] * mult[7];
		matrix[5] = matrix[1] * mult[4] + matrix[5] * mult[5] + matrix[9] * mult[6] + matrix[13] * mult[7];
		matrix[6] = matrix[2] * mult[4] + matrix[6] * mult[5] + matrix[10] * mult[6] + matrix[14] * mult[7];
		matrix[7] = matrix[3] * mult[4] + matrix[7] * mult[5] + matrix[11] * mult[6] + matrix[15] * mult[7];

		matrix[8] = matrix[0] * mult[8] + matrix[4] * mult[9] + matrix[8] * mult[10] + matrix[12] * mult[11];
		matrix[9] = matrix[1] * mult[8] + matrix[5] * mult[9] + matrix[9] * mult[10] + matrix[13] * mult[11];
		matrix[10] = matrix[2] * mult[8] + matrix[6] * mult[9] + matrix[10] * mult[10] + matrix[14] * mult[11];
		matrix[11] = matrix[3] * mult[8] + matrix[7] * mult[9] + matrix[11] * mult[10] + matrix[15] * mult[11];

		matrix[12] = matrix[0] * mult[12] + matrix[4] * mult[13] + matrix[8] * mult[14] + matrix[12] * mult[15];
		matrix[13] = matrix[1] * mult[12] + matrix[5] * mult[13] + matrix[9] * mult[14] + matrix[13] * mult[15];
		matrix[14] = matrix[2] * mult[12] + matrix[6] * mult[13] + matrix[10] * mult[14] + matrix[14] * mult[15];
		matrix[15] = matrix[3] * mult[12] + matrix[7] * mult[13] + matrix[11] * mult[14] + matrix[15] * mult[15];
	}
}
