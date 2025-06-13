using UnityEngine;
using UnityEngine.UIElements;

namespace ATC.Home.UI {
    public class ATC_Home_UI_ExitWarning : ATC_Home_UI_SubClass {

        internal override void Initialize(ATC_Home_UIManager _uiManager, VisualElement _rootElement, string _myRootName) {
            base.Initialize(_uiManager, _rootElement, _myRootName);
            //
            // find reference ui
            Button btnConfirmExit = myRoot.Q<Button>("btnConfirmExit");
            Button btnCancelExit = myRoot.Q<Button>("btnCancelExit");
            //
            // RegisterUICallback
            btnConfirmExit.RegisterCallback<ClickEvent>(OnClick_ConfirmExit);
            btnCancelExit.RegisterCallback<ClickEvent>(OnClick_CancelExit);
        }
        internal override void Activate(bool rBool) {
            base.Activate(rBool);
        }




        private void OnClick_ConfirmExit(ClickEvent e) {
            Application.Quit();
        }
        private void OnClick_CancelExit(ClickEvent e) {
            uiManager.ToggleMainPanel(ATC_Home_PanelType.mainMenu);
        }

    }
}