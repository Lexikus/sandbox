using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using AnimationType = EnemyAnimation.AnimationType;

[RequireComponent(typeof(EnemyAnimation))]
public class EnemyDummy : MonoBehaviour {
    public float SendIntervalRate { get; set; }
    public Vector2 NewPosition { get; set; }

    private EnemyAnimation enemyAnimation;

    // Use this for initialization
    private void Awake() {
        enemyAnimation = GetComponent<EnemyAnimation>();
    }

    private void FixedUpdate() {
        if ((Vector2)transform.position != NewPosition) transform.position = NewPosition;

        // FIXME: Interpolation doesn't work correctly.
        // if ((Vector2)transform.position != NewPosition) {
        //     if (SendIntervalRate == 0) transform.position = NewPosition;
        //     else transform.position = Vector2.Lerp((Vector2)transform.position, NewPosition, SendIntervalRate);
        // }
    }

    public void SetAnimation(AnimationType animationType) {
        enemyAnimation.HandleAnimation(animationType);
    }

    public void Die() {
        enemyAnimation.HandleAnimation(AnimationType.Die);
    }
}
