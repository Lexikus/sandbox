
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Network : MonoBehaviour {
    public static Network Instance { get; private set; }

    protected int hostId = -1;

    protected ConnectionConfig Config { get; private set; }
    protected HostTopology HostTopology { get; private set; }

    public int ReliableChannelId { get; private set; }
    public int UnreliableChannelId { get; private set; }

    private const int MAX_CONNECTIONS = 4;
    protected int port = 5701;

    protected string ipAdress = "127.0.0.1";

    protected bool hasConnectionInitialized;
    protected bool hasConnectionEstablished;

    public const int BUFFER_SIZE = 700;
    public const float GAME_TICK = 0.1f;

    public List<NetworkEntity> Connections { get; protected set; }

    public NetworkDataEvent DataEvent { get; set; }
    public NetworkConnectEvent ConnectEvent { get; set; }
    public NetworkDisconnectEvent DisconnectEvent { get; set; }

    public bool IsServer { get; protected set; }

    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this) Destroy(this);
    }

    protected virtual void Start() {
        ipAdress = NetworkConfig.IPAdress;
        port = NetworkConfig.Port;

        Connections = new List<NetworkEntity>();
        NetworkTransport.Init();

        Config = new ConnectionConfig();
        ReliableChannelId = Config.AddChannel(QosType.Reliable);
        UnreliableChannelId = Config.AddChannel(QosType.Unreliable);

        HostTopology = new HostTopology(Config, MAX_CONNECTIONS);

        DataEvent = new NetworkDataEvent();
        ConnectEvent = new NetworkConnectEvent();
        DisconnectEvent = new NetworkDisconnectEvent();
    }

    public virtual void Send(string message, int channelId) { }
}
