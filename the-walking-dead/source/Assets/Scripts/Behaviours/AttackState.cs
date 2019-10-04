using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Behaviour = Zombie.Behaviour;

public class AttackState : State<Zombie> {
    private const float RUNNING_SPEED = 12;

    public AttackState(Zombie self) : base(self) { }

    public override void OnStateEnter() {
        self.State = Behaviour.Attack;
        self.Agent.speed = RUNNING_SPEED;
    }

    public override void Tick() {
        Chase();
        Attack();
    }

    private void Chase() {
        self.Agent.destination = Player.Instance.transform.position;
    }

    private void Attack() {
        Vector3 distance = (Player.Instance.transform.position - self.transform.position);
        if (distance.magnitude <= 1f) {
            Player.Instance.Kill();
        }
    }
}