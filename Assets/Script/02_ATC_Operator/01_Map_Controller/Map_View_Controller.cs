using UnityEngine;
using UnityEngine.UIElements;

namespace ATC.Operator.MapView {
    [System.Serializable]   
    internal class Map_View_Controller {
        [Tooltip("This will be the heading name, on the top left corner of the map ui panel")]
        [SerializeField] internal string headingCaption;
        [Tooltip("This is the root name of this map controller ui, under uiDocument")]
        [SerializeField] internal string myRootName;
        [SerializeField] internal MapTypeContainer[] allMapTypeContainer;

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
            txtHeading.text = headingCaption;
            drdMapType.choices.Clear();
            for (int i = 0; i < allMapTypeContainer.Length; i++) {
                drdMapType.choices.Add(allMapTypeContainer[i].caption);
            }
            drdMapType.RegisterValueChangedCallback<string>(OnDrdChange_MapType);
            int curMapType = PlayerPrefs.GetInt(headingCaption + nameof(drdMapType), 0);
            drdMapType.SetValueWithoutNotify(allMapTypeContainer[curMapType].caption);
            SetMapType(curMapType);
        }

        private void OnDrdChange_MapType(ChangeEvent<string> evt) {
            SetMapType(drdMapType.index);
        }

        internal virtual void SetMapType(int index) {
            for (int i = 0; i < allMapTypeContainer.Length; i++) {
                allMapTypeContainer[i].mapModelRoot.gameObject.SetActive(i == index);
                allMapTypeContainer[i].camera.gameObject.SetActive(i == index);
            }
            PlayerPrefs.SetInt(headingCaption + nameof(drdMapType), index);
        }


    }

    [System.Serializable]
    internal struct MapTypeContainer {
        [Tooltip("UI Dropdown option name")]
        [SerializeField] internal string caption;
        [Tooltip("3D model root of the map mesh. it will be activate/deactivate based on given index")]
        [SerializeField] internal Transform mapModelRoot;
        [Tooltip("Top view camera of the map. it will be activate/deactivate based on given index. also bg color will be changed according to ui theme color")]
        [SerializeField] internal Camera camera;
    }
}