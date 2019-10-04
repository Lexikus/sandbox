using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseWalkablePoints : MonoBehaviour {

	private readonly int[] WALKABLE = { 16, 18, 30, 32 };
	private List<Vector3> walkables;

	private HouseForm form;

	private void Start() {
		walkables = new List<Vector3>();
		form = GetComponent<HouseForm>();

		foreach (var item in WALKABLE) {
			Vector2 localPosition = MatrixHelper.MatrixPositionToLocalPosition(item, form.Width, form.Height);
			Vector3 worldPosition = transform.TransformPoint(new Vector3(localPosition.x, 1, localPosition.y));

			walkables.Add(worldPosition);
		}
	}

	public Vector3 GetWalkablePosition() {
		int length = walkables.Count;
		return walkables[Random.Range(0, length)];
	}
}
