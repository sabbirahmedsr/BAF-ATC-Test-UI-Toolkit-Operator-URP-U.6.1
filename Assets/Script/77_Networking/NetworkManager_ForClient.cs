using ATC.Global;
using Riptide.Utils;
using Riptide;
using System;
using UnityEngine;

namespace ATC.Operator.Networking {
    public enum MessageID : ushort {
        serverId = 0,
        clientId = 1,
    }

    public class NetworkManager_ForClient : MonoBehaviour {
        private Client client = new Client();
        private int timeOutTimeInMili = 600000;

        [Header("Update Rate")]
        [Tooltip("Number of frame per seconds [network update frame]")]
        [SerializeField] private int frameRate = 5;
        private float frameWaitingTime = 10f;
        private float counter = 0;

        [SerializeField] private Heartbeat heartbeat = new Heartbeat();
        [SerializeField] private NetworkUI networkUI = new NetworkUI();

        #region StartStopUpdate
        internal void Start() {
            RiptideLogger.Initialize(Debug.Log, Debug.Log, Debug.LogWarning, Debug.LogError, false);
            client.Connected += OnSelfConnected;
            client.Disconnected += OnSelfDisconnected;
            client.ConnectionFailed += OnSelfConnectionFailed;
            networkUI.Initialize(this);
            StartClient();
            frameWaitingTime = 1f / frameRate;
        }
        public void Update() {
            counter += Time.deltaTime;
            if (counter >= frameWaitingTime) {
                client.Update(); 
                heartbeat.ManualUpdate();
                counter = 0f;
            }
        }
        #endregion

        #region PublicFunction
        public void StartClient() {
            networkUI.OnStartConnection();
            client.Connect(GlobalNetwork.ip + ":" + GlobalNetwork.port);
            client.TimeoutTime = timeOutTimeInMili;
        }
        public void StopClient() {
            client.Disconnect();
        }
        public void OnApplicationQuit() {
            StopClient();
        }
        #endregion




        #region ConnectionCheck
        public void OnSelfConnected(object sender, EventArgs e) {
            GlobalNetwork.OnConnectionChange(true, e.ToString());
            networkUI.OnConnectionSuccess();
        }
        public void OnSelfDisconnected(object sender, DisconnectedEventArgs e) {
            GlobalNetwork.OnConnectionChange(false, "Server Lost: " + e.Reason);
            networkUI.OnConnectionLost(e.Reason.ToString());
        }
        public void OnSelfConnectionFailed(object sender, ConnectionFailedEventArgs e) {
            GlobalNetwork.OnConnectionChange(false, "No server found: " + e.Reason);
            networkUI.OnConnectionFailure(e.Reason.ToString());
        }
        #endregion





        #region SendOrReceiveMessage
        [MessageHandler((ushort)MessageID.clientId)]
        private static void HandleMessageFromClient(ushort fromClientID, Message message) {
            if (fromClientID == GlobalNetwork.networkManager.client.Id)
                return;
            GlobalNetwork.OnMessageRecieved(message);
        }
        [MessageHandler((ushort)MessageID.serverId)]
        private static void HandleMessageFromServer(Message message) {
            ushort fromClientID = message.GetUShort();
            if (fromClientID == GlobalNetwork.networkManager.client.Id)
                return;
            GlobalNetwork.OnMessageRecieved(message);
        }
        public void SendMessageToNetwork(Message rMsg) {
            client.Send(rMsg);
        }
        #endregion

    }
}
