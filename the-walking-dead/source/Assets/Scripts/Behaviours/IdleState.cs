using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Behaviour = Zombie.Behaviour;

public class IdleState : State<Zombie> {
    public IdleState(Zombie self) : base(self) { }

    public override void OnStateEnter() {
        self.State = Behaviour.Idle;

        GetNewPosition();

        self.SetState(new WalkState(self));
    }

    private void GetNewPosition() {
        Vector3 position = Walkable.Instance.GetWalkablePosition();
        self.Agent.destination = position;
    }
}
