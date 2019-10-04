using UnityEngine;

public class HouseTriggerPressure : MonoBehaviour {
	private HouseForm form;

	private readonly int[] PRESSURE = { 2 };
	private readonly int[] PRESSURE_HEIGHT = { 1 };
	private readonly float[] PRESSURE_SIZE = { .1f };

	private void Start() {
		form = GetComponent<HouseForm>();

		CreatePressure();
	}

	private void CreatePressure() {
		for (int i = 0; i < PRESSURE.Length; i++) {
			int item = PRESSURE[i];
			float height = PRESSURE_HEIGHT[i] - (PRESSURE_HEIGHT[i] - PRESSURE_SIZE[i]) / 2;
			float size = PRESSURE_SIZE[i];

			GameObject g = GameObject.CreatePrimitive(PrimitiveType.Cube);
			g.name = "Pressure";
			g.transform.SetParent(transform);

			g.transform.localScale = new Vector3(1, size, 1);

			Vector2 position = MatrixHelper.MatrixPositionToLocalPosition(item, form.Width, form.Height);
			g.transform.localPosition = new Vector3(position.x, height, position.y);

			g.AddComponent<Pressure>();
		}

	}
}