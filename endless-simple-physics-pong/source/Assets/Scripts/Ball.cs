using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {

	public static Ball Instance { get; private set; }

	public Trans _transform { get; private set; }

	[SerializeField] private Floor[] floors;
	[SerializeField] private Circle[] circles;
	[SerializeField] private Box[] pongs;
	[SerializeField] private Box[] goals;

	private float ballRadius = 0.5f;
	private float bounciness = 0.99f;

	private Vec3 gravityVelocity = Vec3.CreateFromUnityVector3(Physics.gravity);

	private Vec3 velocity;

	private float NO_MOVEMENT_THRESHHOLD = 0.5f;
	private float ARROW_FORCE = 600;

	private void Awake() {
		if (Instance == null) {
			Instance = this;
		} else if (Instance != this) {
			Destroy(this);
		}
	}

	private void Start() {
		_transform = GetComponent<Trans>();
	}

	private void FixedUpdate() {
		ActGravityOnVelocity();
		ActVelocityDirectionOnCollision();
		Movement();
	}

	private void Update() {
		Debug.DrawLine(_transform.Position.ToUnityVector3(), (_transform.Position + velocity).ToUnityVector3());
		OnArrowKey();
		OnSpaceKey();
	}

	private void Movement() {
		if (velocity.Magnitude <= NO_MOVEMENT_THRESHHOLD) return;
		_transform.Translate(velocity * Time.fixedDeltaTime);
	}

	private void ActGravityOnVelocity() {
		velocity += gravityVelocity * Time.fixedDeltaTime;
	}

	private void ActVelocityDirectionOnCollision() {
		CheckFloorCollisions();
		CheckPongCollisions();
		CheckCircleCollisions();
		CheckGoalTriggers();
	}

	private void CheckFloorCollisions() {
		foreach (Floor floor in floors) {
			Vec3 normal = floor.FloorNormal;
			if (CalculatePlaneDistance(normal, _transform.Position, floor._transform.Position) <= ballRadius) {
				velocity = Vec3.CreateFromUnityVector3(Quaternion.AngleAxis(180, normal.ToUnityVector3()) * -velocity.ToUnityVector3());
				velocity = bounciness * velocity;
			}
		}
	}

	private void CheckPongCollisions() {
		foreach (Box pong in pongs) {
			float width = pong.transform.localScale.x;
			float height = pong.transform.localScale.y;
			float depth = pong.transform.localScale.z;
			if (CheckCollisionWithBox(width, height, depth, ballRadius, _transform.Position, pong._transform.Position)) {
				Vec3 normal = pong.FloorNormal;
				velocity = Vec3.CreateFromUnityVector3(Quaternion.AngleAxis(180, normal.ToUnityVector3()) * -velocity.ToUnityVector3());
				velocity = bounciness * velocity;
			}
		}
	}

	private void CheckCircleCollisions() {
		foreach (Circle circle in circles) {
			float radius = circle.Radius;
			if (CalculateCircleDistance(radius, _transform.Position, circle._transform.Position) <= ballRadius) {
				Vec3 spot = GetCircleCollisionSpot(radius, _transform.Position, circle._transform.Position);
				velocity = Vec3.CreateFromUnityVector3(Quaternion.AngleAxis(180, spot.ToUnityVector3()) * -velocity.ToUnityVector3());
				velocity = bounciness * velocity;
				ActArrowForce(velocity.Normalized);
				InvertGravity();
			}
		}
	}

	private void CheckGoalTriggers() {
		foreach (Box goal in goals) {
			float width = goal.transform.localScale.x;
			float height = goal.transform.localScale.y;
			float depth = goal.transform.localScale.z;
			if (CheckCollisionWithBox(width, height, depth, ballRadius, _transform.Position, goal._transform.Position)) {
				if (goal._transform.Position.X < 0) {
					HUD.Instance.AddPointRight();
					ResetBallAfterScore(false);
					return;
				}
				HUD.Instance.AddPointLeft();
				ResetBallAfterScore(true);
				return;
			}
		}
	}

	private float CalculatePlaneDistance(Vec3 normal, Vec3 ball, Vec3 box) {

		float dividend = normal.X * ball.X + normal.Y * ball.Y + normal.Z * ball.Z - (normal.X * box.X + normal.Y * box.Y + normal.Z * box.Z);
		float divisor = normal.Magnitude;

		return dividend / divisor;
	}

	private bool CheckCollisionWithBox(float width, float height, float depth, float ballRadius, Vec3 ball, Vec3 box) {
		Vec3 max = box + new Vec3(height / 2, width / 2, depth / 2);
		Vec3 min = box + new Vec3(-height / 2, -width / 2, -depth / 2);

		float distance = Mathf.Pow(ballRadius, 2);

		if (ball.X < min.X) distance -= Mathf.Pow(ball.X - min.X, 2);
		else if (ball.X > max.X) distance -= Mathf.Pow(ball.X - max.X, 2);
		if (ball.Y < min.Y) distance -= Mathf.Pow(ball.Y - min.Y, 2);
		else if (ball.Y > max.Y) distance -= Mathf.Pow(ball.Y - max.Y, 2);
		if (ball.Z < min.Z) distance -= Mathf.Pow(ball.Z - min.Z, 2);
		else if (ball.Z > max.Z) distance -= Mathf.Pow(ball.Z - max.Z, 2);

		return distance > 0;
	}

	private float CalculateCircleDistance(float circleRadius, Vec3 ball, Vec3 circle) {
		float magnitude = (ball - circle).Magnitude;
		return magnitude - circleRadius;
	}

	private Vec3 GetCircleCollisionSpot(float circleRadius, Vec3 ball, Vec3 circle) {
		Vec3 direction = (ball - circle).Normalized;
		Vec3 collidedSpot = circle + (direction * circleRadius);
		return collidedSpot;
	}

	private void OnArrowKey() {
		Vec3 direction = Vec3.Zero;
		if (Input.GetKeyDown(KeyCode.I)) direction = Vec3.Up;
		else if (Input.GetKeyDown(KeyCode.L)) direction = Vec3.Right;
		else if (Input.GetKeyDown(KeyCode.J)) direction = Vec3.Left;
		else if (Input.GetKeyDown(KeyCode.K)) direction = Vec3.Down;

		if (direction == Vec3.Zero) return;

		ActArrowForce(direction);
	}

	private void OnSpaceKey() {
		if (Input.GetKeyDown(KeyCode.Space)) ResetBall();
	}

	private void ActArrowForce(Vec3 direction) {
		velocity += direction * ARROW_FORCE * Time.deltaTime;
	}

	public void ActExplosionForce(float explosionForce, float areaRadius, Vec3 center, Vec3 ball) {
		float distance = (ball - center).Magnitude;
		if (distance > areaRadius) return;

		Vec3 direction = (ball - center).Normalized;

		float explosionMultiplicator = 1 / distance;

		velocity += direction * explosionMultiplicator * explosionForce * Time.deltaTime;
	}

	private void ResetBall() {
		if (Random.Range(0, 100) % 2 == 0) {
			velocity = Vec3.Right * 10;
			_transform.Position = new Vec3(2f, 13.95f, 0f);
		} else {
			velocity = Vec3.Left * 10;
			_transform.Position = new Vec3(28.0f, 13.95f, 0f);
		}
		_transform.ResetMatrix();
		ResetGravity();
		ResetCooldowns();
	}

	private void ResetBallAfterScore(bool onLeft) {
		velocity = Vec3.Zero;

		if (onLeft) {
			_transform.Position = new Vec3(29.35f, 13.95f, 0f);
		} else {
			_transform.Position = new Vec3(0.65f, 13.95f, 0f);
		}

		_transform.ResetMatrix();
		ResetGravity();
		ResetCooldowns();
	}

	private void ResetGravity() {
		gravityVelocity = Vec3.CreateFromUnityVector3(Physics.gravity);
	}

	private void InvertGravity() {
		gravityVelocity = Vec3.CreateFromUnityVector3(-Physics.gravity);
	}

	private void ResetCooldowns() {
		foreach (Box item in pongs) {
			item.GetComponent<Pong>().ResetExplosionCooldown();
		}
	}
}
