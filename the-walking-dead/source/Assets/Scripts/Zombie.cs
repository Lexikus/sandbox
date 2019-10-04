using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : MonoBehaviour {
    private State<Zombie> state;
    public enum Behaviour {
        Idle,
        Walk,
        Chase,
        Attack
    }
    public Behaviour State { get; set; }
    public NavMeshAgent Agent { get; private set; }

    private void Start() {
        Agent = GetComponent<NavMeshAgent>();

        SetState(new IdleState(this));
    }

    public void SetState(State<Zombie> state) {
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
}
