using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class HUB : MonoBehaviour {


    private void Start() {
    }

    public void OnStartGameClick() {
        ChangeIPAdress("127.0.0.1");
        SceneManager.LoadScene("Server");
    }

    public void OnJoinGameClick() {
        SceneManager.LoadScene("Client");
    }

    public void ChangeIPAdress(string ipaddress) {
        NetworkConfig.IPAdress = ipaddress;
    }
}
