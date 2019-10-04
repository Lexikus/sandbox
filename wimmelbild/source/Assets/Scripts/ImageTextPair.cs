using UnityEngine;
using UnityEngine.UI;

public class ImageTextPair : MonoBehaviour {
    [SerializeField] private Text text;
    private GameObject parentGameObject;

    private void Start() {
        parentGameObject = transform.parent.gameObject;
    }

    public void OnClick() {
        Controller.Instance.TriggerFoundItem();
        parentGameObject.SetActive(false);
        text.text = "<color=#00ff00ff>" + text.text + "</color>";
        Destroy(this);
    }
}
