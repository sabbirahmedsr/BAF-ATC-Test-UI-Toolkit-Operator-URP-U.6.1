using ATC.Operator.Airplane;
using UnityEngine;
using UnityEngine.UIElements;
namespace ATC.Operator.MapView {
    [System.Serializable]
    public class Map_Node {
        /* Note
        * We will only update the aiplance icon position 
        * it will auto update the draggable position using OnGeometryChangeEvent
        * the draggable will update the connecting line using OnGeometryChangeEvent
        */

        internal AirplaneController apController;

        // Internal reference to the TemplateContainer if needed for other operations (e.g., querying nested elements)
        [Header("Root and container")]        
        private TemplateContainer mapNode_CloneElement;
        private Map_Node_AirplaneIcon airplaneIcon = new Map_Node_AirplaneIcon();
        private Map_Node_Draggable draggable = new Map_Node_Draggable();
        private Map_Node_Connecting_Line connectingLine = new Map_Node_Connecting_Line(Vector2.zero, Vector2.zero);
        private Map_Node_Info_UI infoUI = new Map_Node_Info_UI();

        internal Map_Node_Controller mapNodeController;

        public Map_Node(Map_Node_Controller rMapNodeController, AirplaneController rApController) {
            mapNodeController = rMapNodeController;
            apController = rApController;
            CloneAndProcessMapNodeAsync(rMapNodeController.mapNodeTemplate, rMapNodeController.mapNodeContainer);
        }
        private async void CloneAndProcessMapNodeAsync(VisualTreeAsset rMapNodeTemplate, VisualElement rParentContainer) {
            // Create a instance from map node template and find root element
            mapNode_CloneElement = rMapNodeTemplate.CloneTree();
            airplaneIcon.Initialize(this, mapNode_CloneElement);
            draggable.Initialize(this, mapNode_CloneElement, rParentContainer);
            infoUI.Initialize(this, mapNode_CloneElement);
            mapNode_CloneElement.Add(connectingLine);

            // Add created instance for cloning in scene view
            rParentContainer.Add(mapNode_CloneElement);

            // Wait for the end of the frame, after layout has been calculated
            await Awaitable.EndOfFrameAsync();

            // register callback
            apController.apEvent.onUpdate_MapNodePosRot.AddListener(OnUpdate_MapNodePosRot);
            apController.apEvent.onUpdate_VizHeadingSpeedFL.AddListener(OnUpdate_VizHeadingSpeedFL);
        }



        /* External call back */
        private void OnUpdate_MapNodePosRot(Vector3 rWorldPos, Vector3 rFwd) {
            /// We will only update the aiplance icon position
            /// aiplance icon --> OnGeometryChangeEvent --> will update the draggable position 
            /// draggable --> OnGeometryChangeEvent --> will update the map connecting line
            airplaneIcon.OnUpdate_MapNodePosRot(rWorldPos, rFwd);
        }
        internal void OnUpdate_VizHeadingSpeedFL(VizHeadSpeedFL rVizHeadSpeedFL) {
            infoUI.SetVizHeadSpeedHeight(rVizHeadSpeedFL);
        }
        internal void OnChange_ActiveMapCamera(Camera rActiveMapCamera) {
            airplaneIcon.OnChange_ActiveMapCamera(rActiveMapCamera);
        }




        /*Internal Cll back */
        internal void OnGeometryChange_AirplaneIcons() {
            draggable.OnAirplanePositionUpdate(airplaneIcon.localBound.center);
        }
        internal void OnGeometryChange_Draggable() {
            connectingLine.UpdateConnectingLine(draggable.localBound, airplaneIcon.localBound.center);
        }


    }
}