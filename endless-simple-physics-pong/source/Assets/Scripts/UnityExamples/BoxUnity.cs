using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxUnity : MonoBehaviour {

	private MeshFilter meshRenderer;
	private Mesh mesh;

	public List<Vector3> FloorNormals { get; private set; }
	public Vector3 FloorNormal { get; private set; }

	protected virtual void Start() {
		meshRenderer = GetComponent<MeshFilter>();
		mesh = meshRenderer.mesh;

		UpdateNormals();
	}

	private List<Vector3> GetAllTransformedNormals() {
		List<Vector3> list = new List<Vector3>();
		foreach (Vector3 normal in mesh.normals) {
			list.Add(transform.TransformDirection(normal).normalized);
		}
		return list;
	}

	public void UpdateNormals() {
		FloorNormals = GetAllTransformedNormals();
		FloorNormal = FloorNormals[5];
	}
}
