using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HUD : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI text;

    public void HealthChange(int health) {
        text.text = health.ToString();
    }
}
