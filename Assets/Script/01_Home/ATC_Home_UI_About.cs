using UnityEngine.UIElements;

namespace ATC.Home.UI {
    public class ATC_Home_UI_About : ATC_Home_UI_SubClass {

        internal override void Initialize(ATC_Home_UIManager _uiManager, VisualElement _rootElement, string _myRootName) {
            base.Initialize(_uiManager, _rootElement, _myRootName);
            //
            // find reference ui
            Button btnBack = myRoot.Q<Button>("btnBack");
            //
            // RegisterUICallback
            btnBack.RegisterCallback<ClickEvent>(OnClick_BackToMenu);
        }
        internal override void Activate(bool rBool) {
            base.Activate(rBool);
        }




        private void OnClick_BackToMenu(ClickEvent e) {
            uiManager.ToggleMainPanel(ATC_Home_PanelType.mainMenu);
        }

    }
}