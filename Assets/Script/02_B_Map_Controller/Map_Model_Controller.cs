using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace ATC.Operator.MapView {
    internal enum MapThemeEnum { none, day, night, satelite }

    [System.Serializable]
    public class Map_Model_Controller {
        internal string[] MapThemeName = new string[] { "NONE", "LITE", "DARK", "S.MAP" };

        [SerializeField] internal Map_Model_Holder[] allMapModelHolder;

        [Header("UI Reference")]
        private DropdownField drdMapType = null;
        private DropdownField drdMapTheme = null;

        private Map_Controller mapController;

        internal void Initialize(Map_Controller _mapController) {
            mapController = _mapController;

            // Find Reference;
            drdMapType = mapController.mapRoot.Q<DropdownField>("drdMapType");
            drdMapTheme = mapController.mapRoot.Q<DropdownField>("drdMapTheme");

            // Setup initial variable
            /// Map type dropdown
            drdMapType.choices.Clear();
            for (int i = 0; i < allMapModelHolder.Length; i++) {
                drdMapType.choices.Add(allMapModelHolder[i].caption);
            }
            /// Map Theme dropdown
            drdMapTheme.choices.Clear();
            drdMapTheme.choices = MapThemeName.ToList();


            // Get set variable from PlayerPrefs
            int curMapType = PlayerPrefs.GetInt(mapController.headingCaption + nameof(drdMapType), 0);
            drdMapType.SetValueWithoutNotify(allMapModelHolder[curMapType].caption);
            int curMapTheme = PlayerPrefs.GetInt(mapController.headingCaption + nameof(drdMapTheme), 0);
            drdMapTheme.SetValueWithoutNotify(MapThemeName[curMapTheme]);
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

        internal void SetMapTypeAndTheme(int _mapTypeIndex, int _mapThemeIndex) {
            for (int i = 0; i < allMapModelHolder.Length; i++) {
                allMapModelHolder[i].Activate(i == _mapTypeIndex, _mapThemeIndex);
            }
            PlayerPrefs.SetInt(mapController.headingCaption + nameof(drdMapType), _mapTypeIndex);
            PlayerPrefs.SetInt(mapController.headingCaption + nameof(drdMapTheme), _mapThemeIndex);
        }
    }
}