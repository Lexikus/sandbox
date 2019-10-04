using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class World : MonoBehaviour {
	public const int WIDTH = 50;
	public const int HEIGHT = 50;

	private const int MAX_FORMS = 15;
	private const int MIN_MARGIN = 100;
	private const int MAX_MARGIN = 200;

	private Node[] worldMatrix = new Node[WIDTH * HEIGHT];

	[SerializeField] private Form[] forms;
	[SerializeField] private Form[] staticForms;
	private List<Form> spawnedForm;

	[SerializeField] private Material cubeMaterial;

	[SerializeField] private GameObject grassObject;

	public static World Instance { get; private set; }

	public Node[] WorldMatrix {
		get {
			return worldMatrix;
		}
	}

	private void Awake() {
		if (Instance == null) {
			Instance = this;
		} else if (Instance != this) {
			Destroy(this);
		}
	}

	private void Start() {
		spawnedForm = new List<Form>();
		CreateGridWorld();

		GenerateWorld();
		ConnectForms();
		CreateNavMesh();
	}

	private void CreateGridWorld() {
		for (int i = 0; i < WIDTH * HEIGHT; i++) {
			Vector2 vec = MatrixPositionToWorldPosition(i);
			worldMatrix[i] = new Node(i, vec);
		}
	}

	private void GenerateHouse(int slot) {
		Form form = GetStaticForm<HouseForm>();

		form = CreateForm(form);
		spawnedForm.Add(form);

		CreateMeshFromForm(slot, form);
		PopulateFormIntoMatrix(slot, form);
	}

	private void GenerateWorld() {
		Form form = GetForm<SingleForm>();

		form = CreateForm(form);
		spawnedForm.Add(form);

		CreateMeshFromForm(0, form);
		PopulateFormIntoMatrix(0, form);

		GenerateHouse(1123);

		for (int i = 2; i < MAX_FORMS; i++) {
			int randomMargin = Random.Range(MIN_MARGIN, MAX_MARGIN);

			form = GetRandomForm();
			if (!IsSpaceAvailable(i * randomMargin, form.Width, form.Height))
				continue;

			form = CreateForm(form);
			spawnedForm.Add(form);

			CreateMeshFromForm(i * randomMargin, form);
			PopulateFormIntoMatrix(i * randomMargin, form);
		}

		GenerateHouse(2193);
	}

	private void PopulateFormIntoMatrix(int pos, Form form) {
		int i = 0;
		for (int y = 0; y < form.Height; y++) {
			for (int x = 0; x < form.Width; x++) {

				int cube = form.FormMatrix[i];
				int connector = form.ConnectorMatrix[i];

				i++;

				if (connector == 1) worldMatrix[y * WIDTH + x + pos].Value = NodeValue.Connector;
				else worldMatrix[y * WIDTH + x + pos].Value = (NodeValue)cube;
			}
		}
	}

	public List<Node> GetNeighbours(int pos) {
		List<Node> nodes = new List<Node>();

		for (int y = -1; y <= 1; y++) {
			for (int x = -1; x <= 1; x++) {
				if (x == 0 && y == 0) continue;

				if (pos % WIDTH == 0 && x == -1) continue;
				if (pos % WIDTH == WIDTH - 1 && x == 1) continue;

				if (pos + y * WIDTH < 0) continue;
				if (pos + y * WIDTH >= WIDTH * HEIGHT) continue;
				nodes.Add(worldMatrix[y * WIDTH + x + pos]);
			}
		}

		return nodes;
	}

	private void ConnectForms() {
		Form current;
		Vector2 currentWorldPosition = new Vector2(-1, -1);

		Form next;
		Vector2 nextWorldPosition = new Vector2(-1, -1);

		for (int i = 0; i < spawnedForm.Count; i++) {
			current = spawnedForm[i];
			currentWorldPosition = GetFormConnectorWorldPosition(current);

			if (i + 1 >= spawnedForm.Count) return;

			next = spawnedForm[i + 1];
			nextWorldPosition = GetFormConnectorWorldPosition(next);

			if (currentWorldPosition == new Vector2(-1, -1)) return;
			if (nextWorldPosition == new Vector2(-1, -1)) return;

			PathFinding.Find(currentWorldPosition, nextWorldPosition);
			if (PathFinding.Path != null) {
				foreach (var path in PathFinding.Path) {
					CreateCube(path.WorldPosition, true).transform.SetParent(transform);
				}
			}
		}
	}

	private GameObject CreateCube(Vector2 position, bool hasGrass = false) {
		GameObject g = GameObject.CreatePrimitive(PrimitiveType.Cube);
		g.transform.position = new Vector3(position.x, 0, position.y);
		g.isStatic = true;

		Mesh mesh = g.GetComponent<MeshFilter>().mesh;
		PrimitiveHelper.CorrectUVs(mesh);

		MeshRenderer meshRenderer = g.GetComponent<MeshRenderer>();
		meshRenderer.material = cubeMaterial;

		if (hasGrass) CreateGrass(g);
		return g;
	}

	private void CreateGrass(GameObject parent) {
		GameObject createdGrass = Instantiate(grassObject, Vector3.zero, Quaternion.identity);
		createdGrass.transform.SetParent(parent.transform);
		createdGrass.transform.localPosition = new Vector3(Random.Range(-0.5f, 0.5f), .51f, Random.Range(-0.5f, 0.5f));
	}

	private Vector2 GetFormConnectorWorldPosition(Form form) {
		for (int i = 0; i < form.ConnectorMatrix.Length; i++) {
			if (form.ConnectorMatrix[i] == 0) continue;
			Vector3 position = form.transform.GetChild(0).position;
			return LocalPositionToWorldPosition(new Vector2(position.x, position.z), i, form.Width, form.Height);
		}

		return new Vector2(-1, -1);
	}

	private Vector2 LocalPositionToWorldPosition(Vector2 parentPosition, int localPosition, int width, int height) {
		return new Vector2(parentPosition.x + localPosition % width, parentPosition.y + Mathf.Ceil(localPosition / height));
	}

	private Form GetRandomForm() {
		int slot = Random.Range(0, forms.Length);
		return forms[slot];
	}

	private Form GetForm<T>() {
		foreach (var form in forms) {
			if (form.GetType() == typeof(T)) {
				return form;
			}
		}
		return GetRandomForm();
	}

	private Form GetStaticForm<T>() {
		foreach (var form in staticForms) {
			if (form.GetType() == typeof(T)) {
				return form;
			}
		}
		return null;
	}

	private Form CreateForm(Form form) {
		Form _form = Instantiate(form, Vector3.zero, Quaternion.identity);
		_form.gameObject.transform.SetParent(transform);
		return _form;
	}

	private void CreateMeshFromForm(int pos, Form form) {
		int i = 0;
		for (int y = 0; y < form.Height; y++) {
			for (int x = 0; x < form.Width; x++) {
				int worldPos = y * WIDTH + x + pos;
				Vector2 position = MatrixPositionToWorldPosition(worldPos);

				if (x == 0 && y == 0) form.transform.position = new Vector3(position.x, 0, position.y);
				if (form.FormMatrix[i++] == 0) continue;

				CreateCube(position).transform.SetParent(form.transform);
			}
		}
	}

	private bool IsSpaceAvailable(int pos, int width, int height) {
		for (int y = 0; y < height; y++) {
			for (int x = 0; x < width; x++) {
				if (y * WIDTH + x + pos >= worldMatrix.Length) return false;
				if ((int)worldMatrix[y * WIDTH + x + pos].Value > 1) return false;
			}
		}

		return true;
	}


	public Vector2 MatrixPositionToWorldPosition(int pos) {
		return new Vector2(pos % WIDTH, Mathf.Ceil(pos / HEIGHT));
	}

	public int WorldPositionToWorldMatrixPosition(Vector2 pos) {
		return (int)Mathf.Ceil(pos.y) * WIDTH + (int)Mathf.Ceil(pos.x);
	}

	public Node GetNodeFromWorldPosition(Vector2 pos) {
		int i = WorldPositionToWorldMatrixPosition(pos);
		return worldMatrix[i];
	}

	private void CreateNavMesh() {
		NavMeshSurface surface = GetComponent<NavMeshSurface>();
		surface.BuildNavMesh();
	}
}
