using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Behaviour = Minion.Behaviour;

public class IdleState : State<Minion> {
	public IdleState(Minion self) : base(self) { }

	public override void OnStateEnter() {
		self.State = Behaviour.Idle;

		GetNewPosition();

		self.SetState(new WalkState(self));
	}

	private void GetNewPosition() {
		if (self.Agent.enabled == false) return;
		Vector3 position = self.houseWalkablePoints.GetWalkablePosition();
		self.Agent.destination = position;
	}
}
