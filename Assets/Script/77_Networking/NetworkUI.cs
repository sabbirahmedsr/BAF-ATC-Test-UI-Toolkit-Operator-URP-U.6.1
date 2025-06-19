using ATC.Global;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace ATC.Operator.Networking {
    [System.Serializable]
    public class NetworkUI  {
        [SerializeField] private UIDocument uiDocument;
        private VisualElement myRoot;

        private TextElement txtHeading;
        private TextElement txtMessage;

        private Button btnOk;
        private Button btnRetry;
        private Button btnExit;

        private NetworkManager_ForClient networkManager;
        internal void Initialize(NetworkManager_ForClient rNetworkManager) {
            networkManager = rNetworkManager;

            // Find UI Reference
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
        }




        private void OnClick_Ok(ClickEvent evt) {
            ClosePopupMessage();
        }
        private void OnClick_RetryNow(ClickEvent evt) {
            networkManager.StartClient();
        }
        private void OnClick_Exit(ClickEvent evt) {
            SceneManager.LoadScene(GlobalSceneName.home);
        }



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



        private void ShowPopupMessage(string heading, string message, bool showOkButton, bool showRetryButton, bool showExitButton) {
            myRoot.style.display = DisplayStyle.Flex;
            txtHeading.text = heading;
            txtMessage.text = message;
            btnOk.style.display = showOkButton? DisplayStyle.Flex : DisplayStyle.None;
            btnRetry.style.display = showRetryButton ? DisplayStyle.Flex : DisplayStyle.None;
            btnExit.style.display = showExitButton ? DisplayStyle.Flex : DisplayStyle.None; 
        }
        private void ClosePopupMessage() {
            myRoot.style.display = DisplayStyle.None;
        }


    }
}