using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Behaviour = Zombie.Behaviour;

public class WalkState : State<Zombie> {
    private const float WATCH_COOLDOWN = 2f;
    private float watchCooldown = WATCH_COOLDOWN;
    private const float WALK_SPEED = 3.5f;

    public WalkState(Zombie self) : base(self) { }

    public override void OnStateEnter() {
        self.State = Behaviour.Walk;
        self.Agent.speed = WALK_SPEED;
    }

    public override void Tick() {
        Watch();

        if (!HasArrivedDestination()) return;
        self.SetState(new IdleState(self));
    }

    private bool HasArrivedDestination() {
        return self.Agent.remainingDistance <= 1;
    }

    private void Watch() {
        Debug.DrawLine(self.transform.position, Player.Instance.transform.position);
        if (watchCooldown > 0) {
            watchCooldown -= Time.deltaTime;
            return;
        }

        RaycastHit hit;
        Vector3 direction = (Player.Instance.transform.position - self.transform.position).normalized;

        if (Physics.Raycast(self.transform.position, direction, out hit, 100f)) {
            if (hit.transform.CompareTag("Player")) self.SetState(new ChaseState(self));
        }

        watchCooldown = WATCH_COOLDOWN;
    }
}
