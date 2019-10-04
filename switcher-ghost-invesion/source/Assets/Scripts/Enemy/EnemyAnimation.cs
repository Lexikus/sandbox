using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(Animator))]
public class EnemyAnimation : MonoBehaviour {
    public enum AnimationType {
        Idle,
        Die
    }

    [SerializeField] private Animator _animator;

    private void Awake() {
        _animator = GetComponent<Animator>();
    }

    public void HandleAnimation(AnimationType animationType) {
        if (animationType == AnimationType.Die) {
            _animator.SetTrigger("Explode");
        }
    }
}
