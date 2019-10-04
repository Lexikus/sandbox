using UnityEngine;

public class HouseVolume : MonoBehaviour {

	private readonly int[] INDOOR = { 24 };
	private readonly int[] INDOOR_HEIGHT = { 2 };
	private readonly int[] INDOOR_SIZE = { 3 };

	private HouseForm form;

	private void Start() {
		form = GetComponent<HouseForm>();

		CreateVolume();
	}

	private void CreateVolume() {
		for (int i = 0; i < INDOOR.Length; i++) {
			int item = INDOOR[i];
			int height = INDOOR_HEIGHT[i];
			int size = INDOOR_SIZE[i];

			GameObject g = new GameObject("VolumeCube");
			g.transform.SetParent(transform);

			Vector2 position = MatrixHelper.MatrixPositionToLocalPosition(item, form.Width, form.Height);
			g.transform.localPosition = new Vector3(position.x, height, position.y);

			BoxCollider boxCollider = g.AddComponent<BoxCollider>();
			boxCollider.size = new Vector3(size, size, size);
			boxCollider.isTrigger = true;

			g.AddComponent<Volume>();
		}

	}
}