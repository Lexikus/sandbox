using UnityEngine;

public static class MatrixHelper {
	public static Vector2 MatrixPositionToLocalPosition(int pos, int width, int height) {
		return new Vector2(pos % width, Mathf.Ceil(pos / height));
	}
}