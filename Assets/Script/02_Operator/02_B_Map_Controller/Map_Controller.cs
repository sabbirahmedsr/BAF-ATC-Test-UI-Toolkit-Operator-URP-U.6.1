using ATC.Operator.Airplane;
using UnityEngine;
using UnityEngine.UIElements;

namespace ATC.Operator.MapView {
    internal enum MapType { radarMap, surfaceMap}

    public class Map_Controller : ATC_Operator_Sub_Controller {
        [SerializeField] private MapType mapType;
        [Tooltip("The heading name, on the top left corner of the map ui panel")]
        [SerializeField] internal string headingCaption;
        [Tooltip("The root name of Visual Element under under uiDocument")]
        [SerializeField] internal string myRootName;

        [Header("UI Reference")]
        internal VisualElement rootElement;
        internal VisualElement mapRoot;

        [Header("Child Script")]
        [SerializeField] internal Map_Model_Controller mapModelController;
        [SerializeField] internal Map_Node_Controller mapNodeController;
        internal Camera activeMapCamera;


        internal override void Initialize(ATC_Operator_Main_Controller _mainController) {
            base.Initialize(_mainController);

            // Find root, and set heading ui
            rootElement = mainController.uiDocument.rootVisualElement;
            mapRoot = rootElement.Q<VisualElement>(myRootName);            
            TextElement txtHeading = mapRoot.Q<TextElement>("txtHeading");
            txtHeading.text  = headingCaption;

            // Initialize Child Script
            mapModelController.Initialize(this);
            mapNodeController.Initialize(this);

            // Add global listener
            if (mapType == MapType.radarMap) {
                GlobalEvent.onRadarMapNodeCreatedEvent += CreateMapNode;
            } else if (mapType == MapType.surfaceMap) {
                GlobalEvent.onSurfaceMapNodeCreatedEvent += CreateMapNode;
            }
        }

        private void CreateMapNode(AirplaneController rApController, ushort rStartIndex) {
            mapNodeController.CreateMapNode(rApController);            
        }

        internal void OnChange_ActiveMapCamera(Camera rActiveMapCamera) {
            activeMapCamera = rActiveMapCamera;
            mapNodeController.OnChange_ActiveMapCamera(rActiveMapCamera);
        }

    }
}
