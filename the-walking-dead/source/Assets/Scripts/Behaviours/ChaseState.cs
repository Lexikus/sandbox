using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Behaviour = Zombie.Behaviour;

public class ChaseState : State<Zombie> {
    private const float WATCH_COOLDOWN = 0.5f;
    private const float RUNNING_SPEED = 7;

    private float watchCooldown = WATCH_COOLDOWN;

    public ChaseState(Zombie self) : base(self) { }

    public override void OnStateEnter() {
        self.State = Behaviour.Chase;
        self.Agent.speed = RUNNING_SPEED;
    }

    public override void Tick() {
        Watch();
    }

    private bool HasArrivedDestination() {
        return self.Agent.remainingDistance <= 1;
    }

    private void Watch() {
        Debug.DrawLine(self.transform.position, Player.Instance.transform.position, Color.red);
        if (watchCooldown > 0) {
            watchCooldown -= Time.deltaTime;
            return;
        }

        self.Agent.destination = Player.Instance.transform.position;

        RaycastHit hit;
        Vector3 direction = (Player.Instance.transform.position - self.transform.position);

        if (Physics.Raycast(self.transform.position, direction.normalized, out hit, 100f)) {
            if (!hit.transform.CompareTag("Player")) self.SetState(new IdleState(self));
            else {
                if (direction.magnitude <= 3) {
                    self.SetState(new AttackState(self));
                }
            }
        }

        watchCooldown = WATCH_COOLDOWN;
    }
}
