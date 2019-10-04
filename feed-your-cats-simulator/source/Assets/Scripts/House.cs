using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class House : MonoBehaviour {

	[SerializeField] private Material wallMaterial;
	[SerializeField] private Material roofMaterial;
	[SerializeField] private Material doorMaterial;

	private readonly int[] WALLS = { 8, 9, 10, 11, 12, 15, 19, 22, 26, 29, 33, 36, 37, 38, 39, 40 };
	private const int WALL_HEIGHT = 4;

	private const int ROOF_HEIGHT = 4;
	private readonly int[] ROOF = { 16, 17, 18, 23, 24, 25, 30, 31, 32 };

	private const int DOOR_HEIGHT = 2;
	private readonly int[] DOORS = { 10 };
	[SerializeField] private bool openDoorOnCollision;

	private HouseForm form;

	void Start() {
		form = GetComponent<HouseForm>();

		BuildWalls();
		BuildRoof();
		BuildDoors();
	}

	private void BuildWalls() {
		List<int> doorList = DOORS.ToList();
		for (int i = 1; i < WALL_HEIGHT; i++) {
			foreach (var item in WALLS) {
				if (doorList.ToList().Contains(item) && i <= DOOR_HEIGHT) continue;

				GameObject g = GameObject.CreatePrimitive(PrimitiveType.Cube);
				g.transform.SetParent(transform);
				g.isStatic = true;

				Vector2 position = MatrixHelper.MatrixPositionToLocalPosition(item, form.Width, form.Height);

				g.transform.localPosition = new Vector3(position.x, i, position.y);

				Mesh mesh = g.GetComponent<MeshFilter>().mesh;
				MeshRenderer meshRenderer = g.GetComponent<MeshRenderer>();
				meshRenderer.material = wallMaterial;
				PrimitiveHelper.CorrectUVs(mesh);
			}
		}
	}

	private void BuildRoof() {
		foreach (var item in ROOF) {
			GameObject g = GameObject.CreatePrimitive(PrimitiveType.Cube);
			g.transform.SetParent(transform);
			g.isStatic = true;

			Vector2 position = MatrixHelper.MatrixPositionToLocalPosition(item, form.Width, form.Height);

			g.transform.localPosition = new Vector3(position.x, ROOF_HEIGHT, position.y);

			Mesh mesh = g.GetComponent<MeshFilter>().mesh;
			MeshRenderer meshRenderer = g.GetComponent<MeshRenderer>();
			meshRenderer.material = roofMaterial;
			PrimitiveHelper.CorrectUVs(mesh);
		}
	}

	private void BuildDoors() {
		foreach (var item in DOORS) {
			GameObject door = new GameObject("Door");
			door.transform.SetParent(transform);
			door.AddComponent<Door>();

			Vector2 position = MatrixHelper.MatrixPositionToLocalPosition(item, form.Width, form.Height);
			door.transform.localPosition = new Vector3(position.x, 1, position.y);

			for (int i = 1; i <= DOOR_HEIGHT; i++) {
				GameObject g = GameObject.CreatePrimitive(PrimitiveType.Cube);
				g.transform.SetParent(transform);

				position = MatrixHelper.MatrixPositionToLocalPosition(item, form.Width, form.Height);
				g.transform.localPosition = new Vector3(position.x, i, position.y);

				g.transform.SetParent(door.transform);

				Mesh mesh = g.GetComponent<MeshFilter>().mesh;
				MeshRenderer meshRenderer = g.GetComponent<MeshRenderer>();
				meshRenderer.material = doorMaterial;
				PrimitiveHelper.CorrectUVs(mesh);
			}
		}
	}
}
