using ATC.Operator.Airplane;
using System;
using System.Collections;
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

        private Vector2 lastAirplaneScreenPosition = Vector2.positiveInfinity;

        [Header("Child Variable")]
        private VisualElement txtCallSign;
        private VisualElement txtHeading;
        private VisualElement txtSpeed;
        private VisualElement txtHeight;

        private bool isInitialized = false;

        private IPanel containerPanel;
        private Vector2 panelOffset;
        private Camera activeMapCamera;

        public Map_Node(Map_Node_Controller rMapNodeController, AirplaneController rApController) {
            apController = rApController;
            containerPanel = rMapNodeController.mapNodeContainer.panel;
            activeMapCamera = rMapNodeController.mapController.activeMapCamera;
            panelOffset = rMapNodeController.panelOffset;
            rApController.apEvent.onUpdate_MapNodePosRot.AddListener(OnUpdate_MapNodePosRot);
            CloneAndProcessMapNodeAsync(rMapNodeController.mapNodeTemplate, rMapNodeController.mapNodeContainer);
        }
        private async void CloneAndProcessMapNodeAsync(VisualTreeAsset rMapNodeTemplate, VisualElement rParentContainer) {
            // Create a instance from map node template and find root element
            mapNode_CloneElement = rMapNodeTemplate.CloneTree();
            airplaneIcon.Initialize(this, mapNode_CloneElement);
            draggable.Initialize(this, mapNode_CloneElement, rParentContainer);
            mapNode_CloneElement.Add(connectingLine);

            // Add created instance for cloning in scene view
            rParentContainer.Add(mapNode_CloneElement);

            // Wait for the end of the frame, after layout has been calculated
            await Awaitable.EndOfFrameAsync();

            isInitialized = true;
        }





        private void OnUpdate_MapNodePosRot(Vector3 rWorldPos, Vector3 rFwd) {
            if (!isInitialized)
                return;

            Vector2 airplaneScreenPosition = RuntimePanelUtils.CameraTransformWorldToPanel(containerPanel, rWorldPos, activeMapCamera);
            airplaneScreenPosition -= panelOffset;

            // if the airplane screen position is same as the last frame, dont do anything, just skip it
            if (Vector2.Distance(lastAirplaneScreenPosition, airplaneScreenPosition) < 3f) {
                return;
            }

            // if we have new airplane position
            airplaneIcon.OnAirplanePositionUpdate(airplaneScreenPosition);

            // record this screen position to comapre in next frame
            lastAirplaneScreenPosition = airplaneScreenPosition;
        }



        internal void  OnChange_ActiveMapCamera(Camera rActiveMapCamera) {
            activeMapCamera = rActiveMapCamera;
            OnUpdate_MapNodePosRot(apController.position, apController.fwd);
        }




        internal void OnGeometryChange_AirplaneIcons() {
            draggable.OnAirplanePositionUpdate(airplaneIcon.localBound.center);
        }
        internal void OnGeometryChange_Draggable() {
            connectingLine.UpdateConnectingLine(draggable.localBound, airplaneIcon.localBound.center);
        }


    }
}