using UnityEngine.UIElements;

namespace ATC.Home.UI {
    public class ATC_Home_UI_MainMenu : ATC_Home_UI_SubClass {

        internal override void Initialize(ATC_Home_UIManager _uiManager, VisualElement _rootElement, string _myRootName) {
            base.Initialize(_uiManager, _rootElement, _myRootName);
            //
            // find reference ui
            Button btnStart = _rootElement.Q<Button>("btnStart");
            Button btnSetting = _rootElement.Q<Button>("btnSetting");
            Button btnAbout = _rootElement.Q<Button>("btnAbout");
            Button btnExit = _rootElement.Q<Button>("btnExit");
            //
            // RegisterUICallback
            btnStart.RegisterCallback<ClickEvent>(OnClick_Start);
            btnSetting.RegisterCallback<ClickEvent>(OnClick_Setting);
            btnAbout.RegisterCallback<ClickEvent>(OnClick_About);
            btnExit.RegisterCallback<ClickEvent>(OnClick_Exit);
        }
        internal override void Activate(bool rBool) {
            base.Activate(rBool);
        }




        private void OnClick_Start(ClickEvent e) {
            uiManager.ToggleMainPanel(ATC_Home_PanelType.sceneSetup);
        }
        private void OnClick_Setting(ClickEvent e) {
            uiManager.ToggleMainPanel(ATC_Home_PanelType.setting);
        }
        private void OnClick_About(ClickEvent e) {
            uiManager.ToggleMainPanel(ATC_Home_PanelType.about);
        }
        private void OnClick_Exit(ClickEvent e) {
            uiManager.ToggleMainPanel(ATC_Home_PanelType.exitWarning);
        }
    }
}