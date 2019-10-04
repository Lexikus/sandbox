using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaggerDummy : MonoBehaviour {
    public float SendIntervalRate { get; set; }
    public Vector2 NewPosition { get; set; }
    public Quaternion NewRotation { get; set; }

    private void FixedUpdate() {
        if ((Vector2)transform.position != NewPosition) transform.position = NewPosition;
        if (transform.rotation != NewRotation) transform.rotation = NewRotation;

        // FIXME: Interpolation doesn't work correctly.
        // if ((Vector2)transform.position != NewPosition) {
        //     if (SendIntervalRate == 0) transform.position = NewPosition;
        //     else transform.position = Vector2.Lerp((Vector2)transform.position, NewPosition, SendIntervalRate);
        // }

        // if (transform.rotation != NewRotation) {
        //     if (SendIntervalRate == 0) transform.rotation = NewRotation;
        //     else transform.rotation = Quaternion.Lerp(transform.rotation, NewRotation, SendIntervalRate);
        // }
    }
}
