using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Minion : MonoBehaviour {

	private bool hasFeeded = false;
	private State<Minion> state;
	public enum Behaviour {
		Idle,
		Walk,
		Chase,
		Attack
	}
	public Behaviour State { get; set; }
	public NavMeshAgent Agent { get; private set; }

	public HouseWalkablePoints houseWalkablePoints { get; private set; }

	private void Start() {
		houseWalkablePoints = GetComponentInParent<HouseWalkablePoints>();

		Agent = GetComponent<NavMeshAgent>();

		SetState(new IdleState(this));
	}

	public void SetState(State<Minion> state) {
		if (this.state != null)
			this.state.OnStateExit();

		this.state = state;

		if (this.state != null)
			this.state.OnStateEnter();
	}

	private void Update() {
		if (state == null) return;
		state.Tick();
	}

	public bool Feed() {
		if (hasFeeded) return false;
		hasFeeded = true;
		return true;
	}
}
