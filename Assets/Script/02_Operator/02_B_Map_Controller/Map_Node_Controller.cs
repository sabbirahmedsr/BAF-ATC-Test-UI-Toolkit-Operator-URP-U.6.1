using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UIElements;
using ATC.Operator.Airplane;
using System;

namespace ATC.Operator.MapView {
    [System.Serializable]
    public class Map_Node_Controller {
        [SerializeField] internal VisualTreeAsset mapNodeTemplate;
        internal VisualElement mapNodeContainer;


        [SerializeField] private Vector2 panelOffsetRatio = new Vector2(0.5f, 0.5f);
        internal Vector2 panelOffset;

        private List<Map_Node> activeMapNode = new List<Map_Node>();
        private List<Map_Node> mapNodePool = new List<Map_Node>();

        internal Map_Controller mapController;

        internal void Initialize(Map_Controller _mapController) {
            mapController = _mapController;
            //
            mapNodeContainer = _mapController.mapRoot.Q<VisualElement>("mapNodeContainer");
            PanelSettings panelSetting = mapController.mainController.uiDocument.panelSettings;
            panelOffset = new Vector2(Screen.width * panelOffsetRatio.x, Screen.height * panelOffsetRatio.y) / panelSetting.scale;
        }

        internal void OnChange_ActiveMapCamera(Camera rActiveMapCamera) {
            foreach (Map_Node mapNode in activeMapNode) {
                mapNode.OnChange_ActiveMapCamera(rActiveMapCamera);
            }
        }

        internal void CreateMapNode(AirplaneController rApController) {
            Map_Node newMapNode = new Map_Node(this, rApController);
            activeMapNode.Add(newMapNode);
        }
    }
}