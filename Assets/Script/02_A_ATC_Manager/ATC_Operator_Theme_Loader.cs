using ATC.Data;
using UnityEngine;
using UnityEngine.UIElements;

namespace ATC.Operator {
    [System.Serializable]
    public class ATC_Operator_Theme_Loader {
        [SerializeField] private UI_Theme_Data uiThemeData;

        internal void Initialize(ATC_Operator_Main_Controller _mainController) {
            VisualElement rootElement = _mainController.uiDocument.rootVisualElement;
            rootElement.styleSheets.Clear();
            rootElement.styleSheets.Add(uiThemeData.selectedUITheme.styleSheet);

            for (int i = 0; i < _mainController.radarMapController.mapViewController.allMapModelHolder.Length; i++) {
                Camera mapCam = _mainController.radarMapController.mapViewController.allMapModelHolder[i].mapCamera;
                mapCam.backgroundColor = uiThemeData.selectedUITheme.backgroundColor;
            }

            for (int i = 0; i < _mainController.surfaceMapController.mapViewController.allMapModelHolder.Length; i++) {
                Camera mapCam = _mainController.surfaceMapController.mapViewController.allMapModelHolder[i].mapCamera;
                mapCam.backgroundColor = uiThemeData.selectedUITheme.backgroundColor;
            }
        }
    }
}