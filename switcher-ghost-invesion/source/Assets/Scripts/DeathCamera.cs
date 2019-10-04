using UnityEngine;

public class DeathCamera : MonoBehaviour {
    public static DeathCamera Instance { get; private set; }

    [SerializeField] private Camera _camera;

    private void Awake() {
        if (Instance == null) Instance = this;
        else if (Instance != this) Destroy(this);
    }

    public void Enable() {
        _camera.gameObject.SetActive(true);
    }

    public void Disable() {
        _camera.gameObject.SetActive(false);
    }
}