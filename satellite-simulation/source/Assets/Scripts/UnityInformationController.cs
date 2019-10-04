using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnityInformationController : MonoBehaviour {

    private Dictionary<string, string> activeInformations;
    [SerializeField] private Text satelliteMassText;
    [SerializeField] private Text velocityText;
    [SerializeField] private Text accelerationText;
    [SerializeField] private Text radiusText;
    [SerializeField] private Text satelliteCountText;

    private void Update() {
        GetInformationFromSatellite();
        UpdateTextInformations();
        UpdateSatelliteCount();
    }

    private void UpdateTextInformations() {
        if (activeInformations == null) return;
        satelliteMassText.text = activeInformations["satellitemass"];
        velocityText.text = activeInformations["velocity"];
        accelerationText.text = activeInformations["acceleration"];
        radiusText.text = activeInformations["radius"];
    }

    private void GetInformationFromSatellite() {
        if (Input.GetMouseButtonDown(0)) {
            RaycastHit hit;
            Ray mouseRayPosition = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(mouseRayPosition, out hit)) {
                if (!hit.collider.CompareTag("Satellite")) return;

                activeInformations = hit.collider.GetComponent<SatelliteController>().GetInformations();
            }
        }
    }

    private void UpdateSatelliteCount() {
        // the moon and the earth are bodies too.
        satelliteCountText.text = (Body.BodyCounts() - 2).ToString();
    }
}
