using UnityEngine;
using Riptide;
using ATC.Global;

namespace ATC.Operator.Networking{
    [System.Serializable]
    public class Heartbeat {
        [Tooltip("Hearbeat interval in seconds")]
        [SerializeField] internal float heartbeatInterval = 5f;
        private float lastHeartbeatTime;

        internal void ManualUpdate() {
            if (Time.time - lastHeartbeatTime >= heartbeatInterval) {
                if (GlobalNetwork.connectionStatus == ConnectionStatus.connected) {
                    SendHeartbeat();
                }
                lastHeartbeatTime = Time.time;
                Debug.Log("ASDASD");
            }
        }

        private void SendHeartbeat() {
            Message heartBeatMsg = Message.Create(MessageSendMode.Unreliable, MessageID.clientId);
            heartBeatMsg.AddUShort((ushort)MSG_Initial.heartbeat);
            GlobalNetwork.SendMessageToNetwork(heartBeatMsg);
        }

    }
}