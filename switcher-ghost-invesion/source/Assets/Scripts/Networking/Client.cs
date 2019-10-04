using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class Client : Network {
    private int connectionId;

    public int Id { get; private set; }

    protected override void Start() {
        base.Start();

        InitConnection();
    }

    private void InitConnection() {
        byte error;

        hostId = NetworkTransport.AddHost(HostTopology);
        connectionId = NetworkTransport.Connect(hostId, ipAdress, port, 0, out error);

        if ((NetworkError)error != NetworkError.Ok) {
            NetworkError networkError = (NetworkError)error;
            Debug.LogError(networkError.ToString());
        }

        IsServer = false;
        hasConnectionInitialized = true;
    }

    private void OnConnectionEstablished() {
        SceneManager.LoadScene("Game");
    }

    private void OnConnectionDisestablished() {
        byte error;
        NetworkTransport.Disconnect(hostId, connectionId, out error);

        Destroy(gameObject);

        SceneManager.LoadScene("HUB");
    }

    private void Update() {

        if (!hasConnectionInitialized) return;

        int receivedHostId;
        int receivedConnectionId;
        int receivedChannelId;
        byte[] receivedBuffer = new byte[BUFFER_SIZE];
        int receivedDataSize;
        byte error;

        NetworkEventType receivedData = NetworkEventType.Nothing;
        do {
            receivedData = NetworkTransport.Receive(
                out receivedHostId,
                out receivedConnectionId,
                out receivedChannelId,
                receivedBuffer,
                BUFFER_SIZE,
                out receivedDataSize,
                out error
            );

            // TODO: ErrorHandling is missing!

            switch (receivedData) {
                case NetworkEventType.DataEvent:
                    OnData(receivedBuffer, 0, receivedDataSize);
                    break;
                case NetworkEventType.ConnectEvent:
                    hasConnectionEstablished = true;
                    OnConnectionEstablished();
                    break;
                case NetworkEventType.DisconnectEvent:
                    hasConnectionEstablished = false;
                    OnConnectionDisestablished();
                    break;
            }
        } while (receivedData != NetworkEventType.Nothing);
    }

    private void OnData(byte[] buffer, int dataOffset, int dataSize) {
        string message = Encoding.Unicode.GetString(buffer, dataOffset, dataSize);

        string[] data = message.Split(new char[] { '|' }, 2);
        switch (data[0]) {
            case MessageHeaders.PLAYER_IDENTIFIER:
                Id = int.Parse(data[1]);
                break;
        }

        DataEvent.Invoke(message);
    }

    public override void Send(string message, int channelId) {
        if (!hasConnectionEstablished) return;
        // Debug.Log("SENDING: " + message);
        byte[] messageBuffer = Encoding.Unicode.GetBytes(message);

        byte error;
        NetworkTransport.Send(hostId, connectionId, channelId, messageBuffer, message.Length * sizeof(char), out error);
    }
}