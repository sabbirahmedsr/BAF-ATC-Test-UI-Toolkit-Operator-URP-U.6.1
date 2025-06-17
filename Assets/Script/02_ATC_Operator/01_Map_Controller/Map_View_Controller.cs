using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace ATC.Operator.MapView {
    [System.Serializable]
    internal class Map_View_Controller {
        internal enum MapThemeEnum { none, day, night, satelite }
        internal string[] MapThemeName = new string[] { "NONE", "LITE", "DARK", "S.MAP" };

        [Tooltip("This will be the heading name, on the top left corner of the map ui panel")]
        [SerializeField] internal string headingCaption;
        [Tooltip("This is the root name of this map controller ui, under uiDocument")]
        [SerializeField] internal string myRootName;
        [SerializeField] internal Map_Container[] allMapContainer;

        [Header("UI Reference")]
        private TextElement txtHeading = null;
        private DropdownField drdMapType = null;
        private DropdownField drdMapTheme = null;

        internal void Initialize(UIDocument _uiDocument) {
            VisualElement myRoot = _uiDocument.rootVisualElement.Q<VisualElement>(myRootName); 

            // Find Reference;
            txtHeading = myRoot.Q<TextElement>("txtHeading");
            drdMapType = myRoot.Q<DropdownField>("drdMapType");
            drdMapTheme = myRoot.Q<DropdownField>("drdMapTheme");


            // Setup initial variable
            /// heading
            txtHeading.text = headingCaption;
            /// Map type dropdown
            drdMapType.choices.Clear();
            for (int i = 0; i < allMapContainer.Length; i++) {
                drdMapType.choices.Add(allMapContainer[i].caption);
            }
            /// Map Theme dropdown
            drdMapTheme.choices.Clear();
            drdMapTheme.choices = MapThemeName.ToList();


            // Get set variable from PlayerPrefs
            int curMapType = PlayerPrefs.GetInt(headingCaption + nameof(drdMapType), 0);
            drdMapType.index = curMapType;
            int curMapTheme = PlayerPrefs.GetInt(headingCaption + nameof(drdMapTheme), 0);
            drdMapTheme.index = curMapTheme;
            SetMapTypeAndTheme(curMapType, curMapTheme);


            // Register Callback
            drdMapType.RegisterValueChangedCallback<string>(OnDrdChange_MapType);
            drdMapTheme.RegisterValueChangedCallback<string>(OnDrdChange_MapTheme);

        }

        private void OnDrdChange_MapType(ChangeEvent<string> evt) {
            SetMapTypeAndTheme(drdMapType.index, drdMapTheme.index);
        }
        private void OnDrdChange_MapTheme(ChangeEvent<string> evt) {
            SetMapTypeAndTheme(drdMapType.index, drdMapTheme.index);
        }

        internal virtual void SetMapTypeAndTheme(int _mapTypeIndex, int _mapThemeIndex) {
            for (int i = 0; i < allMapContainer.Length; i++) {
                allMapContainer[i].Activate(i == _mapTypeIndex, _mapThemeIndex);
            }
            PlayerPrefs.SetInt(headingCaption + nameof(drdMapType), _mapTypeIndex);
            PlayerPrefs.GetInt(headingCaption + nameof(drdMapTheme), _mapThemeIndex);
        }
    }
}