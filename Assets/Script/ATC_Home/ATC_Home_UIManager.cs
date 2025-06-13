using UnityEngine;
using UnityEngine.UIElements;

namespace ATC.Home.UI {
    internal enum ATC_Home_PanelType { mainMenu, setting, about, exitWarning, sceneSetup }

    [RequireComponent(typeof(UIDocument))]
    public class ATC_Home_UIManager : MonoBehaviour {
        [SerializeField] private ATC_Home_UI_MainMenu _mainMenuUI = new ATC_Home_UI_MainMenu();
        [SerializeField] private ATC_Home_UI_Setting _settingUI = new ATC_Home_UI_Setting();
        [SerializeField] private ATC_Home_UI_About _aboutUI = new ATC_Home_UI_About();
        [SerializeField] private ATC_Home_UI_ExitWarning _exitWarningUI = new ATC_Home_UI_ExitWarning();
        [SerializeField] private ATC_Home_UI_SceneSetup _sceneSetup = new ATC_Home_UI_SceneSetup();

        internal UIDocument uiDocument;
        internal VisualElement rootVisualElement;

        internal void ManualStart() {
            uiDocument = GetComponent<UIDocument>();
            rootVisualElement = uiDocument.rootVisualElement;
            _mainMenuUI.Initialize(this, rootVisualElement, "mainMenuRoot");
            _settingUI.Initialize(this, rootVisualElement, "settingRoot");
            _aboutUI.Initialize(this, rootVisualElement, "aboutRoot");
            _exitWarningUI.Initialize(this, rootVisualElement, "exitWarningRoot");
            _sceneSetup.Initialize(this, rootVisualElement, "sceneSetupRoot");
        }


        internal void ToggleMainPanel(ATC_Home_PanelType _PanelType) {
            _mainMenuUI.Activate(_PanelType == ATC_Home_PanelType.mainMenu);
            _settingUI.Activate(_PanelType == ATC_Home_PanelType.setting);
            _aboutUI.Activate(_PanelType == ATC_Home_PanelType.about);
            _exitWarningUI.Activate(_PanelType == ATC_Home_PanelType.exitWarning);
            _sceneSetup.Activate(_PanelType == ATC_Home_PanelType.sceneSetup);
        }

    }
}