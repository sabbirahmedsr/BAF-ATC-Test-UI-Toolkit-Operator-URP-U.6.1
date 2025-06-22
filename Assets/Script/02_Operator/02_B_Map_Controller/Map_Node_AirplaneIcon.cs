using UnityEngine;
using UnityEngine.UIElements;
namespace ATC.Operator.MapView {

    public class Map_Node_AirplaneIcon {
        internal Rect localBound { get { return apIconRoot.localBound; } }

        private VisualElement apIconRoot;

        [Header("Panel Reference")]
        // the container panel, where this icon well be 
        private IPanel containerPanel;
        // our container panel might not be at zero position of the screen, so we need an offset value to get panel local location
        private Vector2 panelOffset;
        // out camera is full screen, but container has a heading, we need to substract the heading height for accurate icon position
        private Vector2 headingOffset = new Vector2(0, 40);   /// to exclude the heading height from container

        [Header("Camera Reference")]
        // we need reference of active camera, so that we can convert world position into panel position
        private Camera activeMapCamera;
        // sometime, camera might be rotated in y axis, [surface map camera of vghs-254 degree rotated]
        // we need to adjust that rotation to our airplane icon
        private float yRotationOffset;

        [Header("For Optimization")]
        private Vector3 lastWorldPos;
        private Vector3 lastFwd;
        private Vector2 lastScreenPos;

        // reference of parent script
        private Map_Node mapNode;

        internal void Initialize(Map_Node rMapNode, VisualElement rMapNode_CloneElement) {
            // store reference value
            this.mapNode = rMapNode;
            activeMapCamera = mapNode.mapNodeController.mapController.activeMapCamera;
            yRotationOffset = activeMapCamera.transform.eulerAngles.y; /// For Adjusting camera top view rotation
            containerPanel = mapNode.mapNodeController.mapNodeContainer.panel;
            panelOffset = mapNode.mapNodeController.panelOffset;

            // get ui reference
            apIconRoot = rMapNode_CloneElement.Q<VisualElement>("airplaneIcon");

            // register callback
            apIconRoot.RegisterCallback<GeometryChangedEvent>(OnGeometryChange);
        }


        internal void OnUpdate_MapNodePosRot(Vector3 rWorldPos, Vector3 rFwd) {
            // if we have same location and rotation of the previous frame, skip the further operation
            if(rWorldPos == lastWorldPos && rFwd == lastFwd) {
                return;
            }
            // store the position and fwd for comparing in next frame
            lastWorldPos = rWorldPos;
            lastFwd = rFwd;

            // get screen potision
            Vector2 screenPosition = RuntimePanelUtils.CameraTransformWorldToPanel(containerPanel, rWorldPos, activeMapCamera);
            // if the screen position has not changed a pixel, we will skip further calculation
            if (Vector2.Distance(screenPosition, lastScreenPos) < 2) {
                return;
            }
            // store the screen position for next frame comparison
            lastScreenPos = screenPosition;

            // adjust icon width and height to get center pos
            screenPosition -= new Vector2(apIconRoot.layout.width / 2f, apIconRoot.layout.height / 2f);
            // adjust panel offset
            screenPosition -= panelOffset;            
            // adjust panel heading
            screenPosition -= headingOffset;

            // set icon position
            apIconRoot.style.left = screenPosition.x;
            apIconRoot.style.top = screenPosition.y;

            // set icon rotation
            Vector2 forward2D = new Vector2(rFwd.x, rFwd.z);
            float angleFromNorth = Vector2.SignedAngle(Vector2.up, forward2D);
            angleFromNorth -= -yRotationOffset;     /// Adjust camera top view rotation
            apIconRoot.style.rotate = new Rotate(new Angle(-angleFromNorth ));
        }


        internal void OnChange_ActiveMapCamera(Camera rActiveMapCamera) {
            activeMapCamera = rActiveMapCamera;
            lastWorldPos = Vector3.positiveInfinity; lastFwd = Vector3.positiveInfinity;
            OnUpdate_MapNodePosRot(mapNode.apController.position, mapNode.apController.fwd);
        }

        private void OnGeometryChange(GeometryChangedEvent evt) {
            mapNode.OnGeometryChange_AirplaneIcons();
        }
    }
}