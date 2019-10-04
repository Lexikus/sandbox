using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallUnity : MonoBehaviour {

	public static BallUnity instance;

	[SerializeField] private FloorUnity[] floors;
	[SerializeField] private CircleUnity[] circles;
	[SerializeField] private BoxUnity[] pongs;
	[SerializeField] private BoxUnity[] goals;

	private float ballRadius = 0.5f;
	private float bounciness = 0.99f;

	private Vector3 gravityVelocity = Physics.gravity;

	private Vector3 velocity;

	private float NO_MOVEMENT_THRESHHOLD = 0.5f;
	private float ARROW_FORCE = 600;

	private void Awake() {
		instance = this;
	}

	private void FixedUpdate() {
		ActGravityOnVelocity();
		ActVelocityDirectionOnCollision();
		Movement();
	}

	private void Update() {
		Debug.DrawLine(transform.position, transform.position + velocity);
		OnArrowKey();
	}

	private void Movement() {
		if (velocity.magnitude <= NO_MOVEMENT_THRESHHOLD) return;
		transform.transform.Translate(velocity * Time.fixedDeltaTime);
	}

	private void ActGravityOnVelocity() {
		velocity += gravityVelocity * Time.fixedDeltaTime;
	}

	private void ActVelocityDirectionOnCollision() {
		foreach (FloorUnity floor in floors) {
			Vector3 normal = floor.FloorNormal;
			if (CalculatePlaneDistance(normal, transform.position, floor.transform.position) <= ballRadius) {
				velocity = Quaternion.AngleAxis(180, normal) * -velocity;
				velocity = bounciness * velocity;
			}
		}

		foreach (BoxUnity pong in pongs) {
			float width = pong.transform.localScale.x;
			float height = pong.transform.localScale.y;
			float depth = pong.transform.localScale.z;
			if (CheckCollisionWithBox(width, height, depth, ballRadius, transform.position, pong.transform.position)) {
				Vector3 normal = pong.FloorNormal;
				velocity = Quaternion.AngleAxis(180, normal) * -velocity;
				velocity = bounciness * velocity;
			}
		}

		foreach (CircleUnity circle in circles) {
			if (circle.IgnoreCollision) return;
			float radius = circle.Radius;
			if (CalculateCircleDistance(radius, transform.position, circle.transform.position) <= ballRadius) {
				Vector3 spot = GetCircleCollisionSpot(radius, transform.position, circle.transform.position);
				velocity = Quaternion.AngleAxis(180, spot) * -velocity;
				velocity = bounciness * velocity;
				ActArrowForce(velocity.normalized);
				circle.SetCollided();
			}
		}

		foreach (BoxUnity goal in goals) {
			float width = goal.transform.localScale.x;
			float height = goal.transform.localScale.y;
			float depth = goal.transform.localScale.z;
			if (CheckCollisionWithBox(width, height, depth, ballRadius, transform.position, goal.transform.position)) {
				Debug.Log("Goal");
			}
		}
	}

	private float CalculatePlaneDistance(Vector3 normal, Vector3 ball, Vector3 box) {
		float magnitude = (Mathf.Sqrt(Mathf.Pow(normal.x, 2) + Mathf.Pow(normal.y, 2) + Mathf.Pow(normal.z, 2))); // normal.magnitude

		float dividend = normal.x * ball.x + normal.y * ball.y + normal.z * ball.z - (normal.x * box.x + normal.y * box.y + normal.z * box.z);
		float divisor = magnitude;

		return dividend / divisor;
	}

	// aabb collision
	private bool CheckCollisionWithBox(float width, float height, float depth, float ballRadius, Vector3 ball, Vector3 box) {
		Vector3 max = box + new Vector3(height / 2, width / 2, depth / 2);
		Vector3 min = box + new Vector3(-height / 2, -width / 2, -depth / 2);

		float distance = Mathf.Pow(ballRadius, 2);

		if (ball.x < min.x) distance -= Mathf.Pow(ball.x - min.x, 2);
		else if (ball.x > max.x) distance -= Mathf.Pow(ball.x - max.x, 2);
		if (ball.y < min.y) distance -= Mathf.Pow(ball.y - min.y, 2);
		else if (ball.y > max.y) distance -= Mathf.Pow(ball.y - max.y, 2);
		if (ball.z < min.z) distance -= Mathf.Pow(ball.z - min.z, 2);
		else if (ball.z > max.z) distance -= Mathf.Pow(ball.z - max.z, 2);

		return distance > 0;
	}

	private float CalculateCircleDistance(float circleRadius, Vector3 ball, Vector3 circle) {
		float magnitude = (Mathf.Sqrt(
			Mathf.Pow(ball.x - circle.x, 2)
			+ Mathf.Pow(ball.y - circle.y, 2)
			+ Mathf.Pow(ball.z - circle.z, 2)
		));
		return magnitude - circleRadius;
	}

	private Vector3 GetCircleCollisionSpot(float circleRadius, Vector3 ball, Vector3 circle) {
		Vector3 direction = (ball - circle).normalized;
		Vector3 collidedSpot = circle + (direction * circleRadius);
		return collidedSpot;
	}

	private void OnArrowKey() {
		Vector3 direction = Vector3.zero;
		if (Input.GetKeyDown(KeyCode.I)) direction = Vector3.up;
		else if (Input.GetKeyDown(KeyCode.L)) direction = Vector3.right;
		else if (Input.GetKeyDown(KeyCode.J)) direction = Vector3.left;
		else if (Input.GetKeyDown(KeyCode.K)) direction = Vector3.down;

		if (direction == Vector3.zero) return;

		ActArrowForce(direction);
	}

	private void ActArrowForce(Vector3 direction) {
		velocity += direction * ARROW_FORCE * Time.deltaTime;
	}

	public void ActExplosionForce(float explosionForce, float areaRadius, Vector3 center, Vector3 ball) {
		float distance = (ball - center).magnitude;
		if (distance > areaRadius) return;

		Vector3 direction = (ball - center).normalized;

		float A = 1 / distance;

		velocity += direction * A * explosionForce * Time.deltaTime;
	}
}
