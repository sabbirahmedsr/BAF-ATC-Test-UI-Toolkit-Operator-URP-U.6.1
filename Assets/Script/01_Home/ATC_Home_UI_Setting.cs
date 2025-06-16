using ATC.Data;
using UnityEngine;
using UnityEngine.UIElements;

namespace ATC.Home.UI {
    [System.Serializable]
    public class ATC_Home_UI_Setting : ATC_Home_UI_SubClass {
        [SerializeField] private UI_Theme_Data uiThemeData;

        private DropdownField drdUITheme;   // keep this as reference, because we need to use dropdown index later
        private Slider sldrUIScale;         // keep this as reference, because we need this later

        internal override void Initialize(ATC_Home_UIManager _uiManager, VisualElement _rootElement, string _myRootName) {
            base.Initialize(_uiManager, _rootElement, _myRootName);
            //
            // find reference ui
            drdUITheme = myRoot.Q<DropdownField>("drdUITheme");
            sldrUIScale = myRoot.Q<Slider>("sldrUIScale");
            Button btnBack = myRoot.Q<Button>("btnBack");
            //
            //
            //
            // setup reference ui
            /// Set UI Theme dropdown
            drdUITheme.choices.Clear();
            drdUITheme.choices = uiThemeData.GetAllThemeName();
            int uiThemeIndex = PlayerPrefs.GetInt(drdUITheme.name, 0);
            drdUITheme.SetValueWithoutNotify(drdUITheme.choices[uiThemeIndex]);
            ChangeUITheme(uiThemeIndex);
            /// Set UI Scale Slider
            float uiScaleValue = PlayerPrefs.GetFloat(sldrUIScale.name, 1f);
            sldrUIScale.SetValueWithoutNotify(uiScaleValue);
            ChangeUIScale(uiScaleValue);
            //
            //
            //
            // RegisterUICallback
            btnBack.RegisterCallback<ClickEvent>(OnClick_BackToMenu);
            drdUITheme.RegisterValueChangedCallback<string>(OnDrdChange_UITheme);
            sldrUIScale.RegisterValueChangedCallback<float>(OnSldrChange_UIScale);
        }



        internal override void Activate(bool rBool) {
            base.Activate(rBool);
        }





        private void OnDrdChange_UITheme(ChangeEvent<string> evt) {
            ChangeUITheme(drdUITheme.index);
        }
        private void OnSldrChange_UIScale(ChangeEvent<float> evt) {
            ChangeUIScale(evt.newValue);
        }
        private void OnClick_BackToMenu(ClickEvent e) {
            uiManager.ToggleMainPanel(ATC_Home_PanelType.mainMenu);
        }




        internal void ChangeUITheme(int rIndex) {
            uiThemeData.selectedThemeColor = uiThemeData.GetStyleSheet(rIndex);
            uiManager.rootVisualElement.styleSheets.Clear();
            uiManager.rootVisualElement.styleSheets.Add(uiThemeData.selectedThemeColor);
            PlayerPrefs.SetInt(nameof(drdUITheme), rIndex);
        }
        internal void ChangeUIScale(float rValue) {
            uiManager.uiDocument.panelSettings.scale = rValue;
            PlayerPrefs.SetFloat(sldrUIScale.name, rValue);
        }
    }
}