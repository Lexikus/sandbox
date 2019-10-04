using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterAnimation))]
public class PlayerDummy : MonoBehaviour {

    private CharacterAnimation characterAnimation;
    private HUD characterHUD;
    private GameObject characterHUDGameObject;
    private Camera characterCamera;
    public float SendIntervalRate { get; set; }
    public Vector2 NewPosition { get; set; }

    // Use this for initialization
    private void Awake() {
        characterAnimation = GetComponent<CharacterAnimation>();
        characterCamera = GetComponentInChildren<Camera>();

        var components = GetComponentsInChildren<HUD>(true);
        if (components.Length > 0) {
            characterHUD = GetComponentsInChildren<HUD>(true)[0];
            characterHUDGameObject = characterHUD.gameObject;
        }
    }

    private void FixedUpdate() {
        if ((Vector2)transform.position != NewPosition) transform.position = NewPosition;

        // FIXME: Interpolation doesn't work correctly.
        // if ((Vector2)transform.position != NewPosition) {
        //     if (SendIntervalRate == 0) transform.position = NewPosition;
        //     else transform.position = Vector2.Lerp((Vector2)transform.position, NewPosition, SendIntervalRate);
        // }
    }

    public void SetMovement(Vector2 movement) {
        characterAnimation.HandleMovementAnimaton(movement);
    }

    public void ShowHUD() {
        characterHUDGameObject.SetActive(true);
    }

    public void HideHUD() {
        characterHUDGameObject.SetActive(false);
    }

    public void ChangeHealth(int health) {
        characterHUD.HealthChange(health);
    }

    public void ShowCamera() {
        characterCamera.enabled = true;
    }

    public void HideCamera() {
        characterCamera.enabled = false;
    }
}
