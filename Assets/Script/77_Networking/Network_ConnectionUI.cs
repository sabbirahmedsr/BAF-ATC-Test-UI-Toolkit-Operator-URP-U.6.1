using ATC.Global;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace ATC.Operator.Networking {
    [System.Serializable]
    public class Network_ConnectionUI : MonoBehaviour {
        private VisualElement myRoot;

        [Header("Text Reference")]
        private TextElement txtHeading;
        private TextElement txtMessage;

        [Header("Button Reference")]
        private Button btnOk;
        private Button btnRetry;
        private Button btnExit;

        private NetworkManager_ForClient networkManager;
        internal void Initialize(NetworkManager_ForClient rNetworkManager) {
            networkManager = rNetworkManager;

            // Find UI Reference
            UIDocument uiDocument = FindAnyObjectByType<UIDocument>();
            myRoot = uiDocument.rootVisualElement.Q<VisualElement>("networkUI");
            txtHeading = myRoot.Q<TextElement>("txtHeading");
            txtMessage = myRoot.Q<TextElement>("txtMessage");
            btnOk = myRoot.Q<Button>("btnOk");
            btnRetry = myRoot.Q<Button>("btnRetry");
            btnExit = myRoot.Q<Button>("btnExit");

            // register callback
            btnOk.RegisterCallback<ClickEvent>(OnClick_Ok);
            btnRetry.RegisterCallback<ClickEvent>(OnClick_RetryNow);
            btnExit.RegisterCallback<ClickEvent>(OnClick_Exit);

            ClosePopupMessage();
            GlobalNetwork.onConnectionEvent += OnConnectionChange;
        }




        private void OnClick_Ok(ClickEvent evt) {
            ClosePopupMessage();
        }
        private void OnClick_RetryNow(ClickEvent evt) {
            networkManager.StartOrRetryConnection();
        }
        private void OnClick_Exit(ClickEvent evt) {
            SceneManager.LoadScene(GlobalSceneName.home);
        }



        internal void OnConnectionChange(ConnectionStatus rConnectionStatus, string rHeadingString, string rReason) {
            ShowPopupMessage(rHeadingString, rReason, rConnectionStatus);
        }
        /*
        internal void OnStartConnection() {
            ShowPopupMessage("CONNECTING...", "Trying to connecting the server...", false, false, false);
        }
        internal void OnConnectionSuccess() {
            ShowPopupMessage("SUCCESS", "SUCCESSFULLY CONNECTED TO SERVER !!!", true, false, false);
        }
        internal void OnConnectionFailure(string rReason) {
            ShowPopupMessage("FAILURE", rReason, false, true, true);
        }
        internal void OnConnectionLost(string rReason) {
            ShowPopupMessage("SERVER LOST", rReason, false, true, true);
        }
        */


        private void ShowPopupMessage(string heading, string message, ConnectionStatus rConnectionStatus) {
            myRoot.style.display = DisplayStyle.Flex;
            txtHeading.text = heading;
            txtMessage.text = message;
            btnOk.style.display = rConnectionStatus == ConnectionStatus.connected ? DisplayStyle.Flex : DisplayStyle.None;
            bool showRetryAndExit = rConnectionStatus == ConnectionStatus.disconnected || rConnectionStatus == ConnectionStatus.connectionFailed;
            btnRetry.style.display = showRetryAndExit ? DisplayStyle.Flex : DisplayStyle.None;
            btnExit.style.display = showRetryAndExit ? DisplayStyle.Flex : DisplayStyle.None; 
        }
        private void ClosePopupMessage() {
            myRoot.style.display = DisplayStyle.None;
        }

    }
}