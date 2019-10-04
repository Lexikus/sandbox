using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Networking;
using UnityEngine.UI;

public class MainScreenHost : MonoBehaviour {

    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider volumeSlider;
    [SerializeField] GameObject mainScreenView;
    [SerializeField] GameObject lobbyScreenView;
    [SerializeField] InputField inputFieldIPAddress;
    [SerializeField] Text ipAddress;
    [SerializeField] Text joinButtonText;

    private void Start() {
        GetAndShowIPAddress();
    }

    private void GetAndShowIPAddress() {
        if (ipAddress == null) return;
        ipAddress.text = Network.player.ipAddress;
    }

    public void InitSlideVolumeValue() {
        if (audioMixer == null) return;
        if (volumeSlider == null) return;
        float volume = 0;
        audioMixer.GetFloat("MasterVolume", out volume);
        volumeSlider.value = volume;
    }

    public void ManageVolume(float volume) {
        if (audioMixer == null) return;
        audioMixer.SetFloat("MasterVolume", volume);
    }

    public void ExitGame() {
        Application.Quit();
    }

    public void CreateHost() {
        AutoNetworkManager lobby = NetworkManager.singleton as AutoNetworkManager;
        if (lobby == null) return;
        lobby.CreateHost();
    }

    public void JoinHost() {
        if (inputFieldIPAddress == null) return;
        if (string.IsNullOrEmpty(inputFieldIPAddress.text)) return;

        AutoNetworkManager lobby = NetworkManager.singleton as AutoNetworkManager;
        if (lobby == null) return;

        string ipAddress = inputFieldIPAddress.text;

        lobby.JoinHost(ipAddress);
        if (joinButtonText == null) return;
        joinButtonText.text = "Connecting...";
    }
}
