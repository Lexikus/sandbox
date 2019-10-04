using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class Server : Network {

    [SerializeField] private TextMeshProUGUI terminal;

    protected override void Start() {
        base.Start();

        InitConnection();
    }

    private void InitConnection() {
        hostId = NetworkTransport.AddHost(HostTopology, port);

        // An error occurred. Unity will tell us the error.
        if (hostId == -1) return;

        hasConnectionInitialized = true;
        hasConnectionEstablished = true;
        IsServer = true;

        SceneManager.LoadScene("Game");
    }

    private void Update() {
        if (!hasConnectionEstablished) return;

        HandleReceivedData();
    }

    private void HandleReceivedData() {
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
                case NetworkEventType.ConnectEvent:
                    OnConnect(receivedConnectionId);
                    break;
                case NetworkEventType.DataEvent:
                    OnData(receivedBuffer, 0, receivedDataSize);
                    break;
                case NetworkEventType.DisconnectEvent:
                    OnDisconnect(receivedConnectionId);
                    break;
            }
        } while (receivedData != NetworkEventType.Nothing);
    }

    private void OnConnect(int connectionId) {
        Log("RECEIVING: Player with ID " + connectionId + " connected");

        if (Connections.Find(x => x.ID == connectionId) != null) return;

        Connections.Add(new NetworkEntity() { ID = connectionId, ConnectionTime = Time.time });

        string message = MessageHeaders.PLAYER_IDENTIFIER + "|" + connectionId;
        Send(message, ReliableChannelId, connectionId);

        ConnectEvent.Invoke(connectionId);
    }

    private void OnData(byte[] buffer, int dataOffset, int dataSize) {
        string message = Encoding.Unicode.GetString(buffer, dataOffset, dataSize);
        Log("RECEIVING: " + message);
        DataEvent.Invoke(message);
    }

    private void OnDisconnect(int connectionId) {
        Log("RECEIVING: Player with ID " + connectionId + " disconnected");

        NetworkEntity entity = Connections.Find(x => x.ID == connectionId);
        if (entity == null) return;

        Connections.Remove(entity);
        DisconnectEvent.Invoke(connectionId);
    }

    public override void Send(string message, int channelId) {
        if (!hasConnectionEstablished) return;
        if (channelId == ReliableChannelId) Log("SENDING: " + message);
        byte[] messageBuffer = Encoding.Unicode.GetBytes(message);
        foreach (var entity in Connections) {
            byte error;
            NetworkTransport.Send(hostId, entity.ID, channelId, messageBuffer, message.Length * sizeof(char), out error);
        }
    }

    private void Send(string message, int channelId, int connectionId) {
        if (!hasConnectionEstablished) return;
        NetworkEntity entity = Connections.Find(x => x.ID == connectionId);

        if (entity == null) return;

        Log("SENDING: " + message);
        byte[] messageBuffer = Encoding.Unicode.GetBytes(message);

        byte error;
        NetworkTransport.Send(hostId, entity.ID, channelId, messageBuffer, message.Length * sizeof(char), out error);
    }

    private void Log(string message) {
        // Debug.Log(message);
        // terminal.text = message + "\n" + terminal.text;
    }
}
