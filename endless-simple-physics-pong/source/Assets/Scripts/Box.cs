using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour {

	public Trans _transform { get; private set; }

	private MeshFilter meshRenderer;
	private Mesh mesh;

	public List<Vec3> FloorNormals { get; private set; }
	public Vec3 FloorNormal { get; private set; }

	protected virtual void Start() {
		_transform = GetComponent<Trans>();
		meshRenderer = GetComponent<MeshFilter>();
		mesh = meshRenderer.mesh;

		UpdateNormals();
	}

	private List<Vec3> GetAllTransformedNormals() {
		List<Vec3> list = new List<Vec3>();
		foreach (Vec3 normal in Vec3.CreateFromUnityVector3(mesh.normals)) {
			list.Add(Vec3.CreateFromUnityVector3(transform.TransformDirection(normal.ToUnityVector3())).Normalized);
		}
		return list;
	}

	public void UpdateNormals() {
		FloorNormals = GetAllTransformedNormals();
		FloorNormal = FloorNormals[5];
	}
}
