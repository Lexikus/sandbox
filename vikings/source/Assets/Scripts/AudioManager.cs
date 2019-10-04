using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour {

    // Referenz zum AudioMixer wo alle Sounds sind
	[SerializeField] private AudioMixer volumeMixer;

    // Lautstärke anpassen via Slider
	public void ChangeVolume(float volume){
        volumeMixer.SetFloat("Volume", volume);
    }
}
