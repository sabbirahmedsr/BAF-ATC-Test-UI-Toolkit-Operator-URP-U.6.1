using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UIElements;

namespace ATC.Operator.MapView {
    [System.Serializable]
    public class Map_Node_Controller {
        [SerializeField] internal VisualTreeAsset mapNodeTemplate;
        internal VisualElement mapNodeContainer;


        [SerializeField] private Vector2 initialOffsetRatio = new Vector2(0.5f, 0.5f);
        private Vector2 initialOffset;

        private List<Map_Node> activeMapNode = new List<Map_Node>();
        private List<Map_Node> mapNodePool = new List<Map_Node>();

        private Camera activeMapCamera;

        private Map_Node temporaryMapNode;

        internal void Initialize(Map_Controller _mapController) {
            Map_Controller mapController = _mapController;
            //
            mapNodeContainer = _mapController.mapRoot.Q<VisualElement>("mapNodeContainer");
            temporaryMapNode = new Map_Node(this);
            activeMapNode.Add(temporaryMapNode);

            PanelSettings panelSetting = mapController.mainController.uiDocument.panelSettings;
            initialOffset = new Vector2(Screen.width * initialOffsetRatio.x, Screen.height * initialOffsetRatio.y) / panelSetting.scale;

            activeMapCamera = mapController.activeMapCamera;
        }

        public Transform tempTarget;


        internal void ManualUpdate() {
            Vector2 screenPosition = RuntimePanelUtils.CameraTransformWorldToPanel(
                   mapNodeContainer.panel,
                   tempTarget.position,
                   activeMapCamera
               );
            screenPosition -= initialOffset;
            temporaryMapNode.OnAirplanePositionUpdate(screenPosition);
        }
    }
}