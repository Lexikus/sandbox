using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HouseMinionSpawn : MonoBehaviour {

	private readonly int[] SPAWNABLE = { 24 };
	[SerializeField] private GameObject minion;

	private HouseForm form;

	IEnumerator Start() {
		form = GetComponent<HouseForm>();
		int spawn = GetRandomSpawnable();
		Vector2 localPosition = MatrixHelper.MatrixPositionToLocalPosition(spawn, form.Width, form.Height);
		GameObject spawnedMinion = Instantiate(minion, Vector3.zero, Quaternion.identity);
		spawnedMinion.transform.SetParent(transform);
		spawnedMinion.transform.localPosition = new Vector3(localPosition.x, 1, localPosition.y);

		yield return new WaitForSeconds(2);

		spawnedMinion.GetComponent<NavMeshAgent>().enabled = true;
	}

	private int GetRandomSpawnable() {
		int length = SPAWNABLE.Length;
		return SPAWNABLE[Random.Range(0, length)];
	}
}
