using System;

namespace ATC.Global {
    public enum ConnectionStatus { hasNoResult, connected, connectionFailed }

    public class GlobalNetwork {
        public static string ip = "127.0.0.1";
        public static ushort port = 7777;
        public static ConnectionStatus connectionStatus = ConnectionStatus.hasNoResult;

        // client event
        public static event Action<bool, string> onConnectionEvent = delegate { };
        public static event Action<string> onMessageReceivedEvent = delegate { };

        internal static void OnConnectionChange(bool rSuccessBool, string rMsg) {
            connectionStatus = rSuccessBool ? ConnectionStatus.connected : ConnectionStatus.connectionFailed;
            onConnectionEvent.Invoke(rSuccessBool, rMsg);
        }

        internal static void OnMessageRecieved(string rMsg) {
            onMessageReceivedEvent.Invoke(rMsg);
        }

        internal static void SendMessageToNetwork(string rMsg) {
          
        }

    }
}