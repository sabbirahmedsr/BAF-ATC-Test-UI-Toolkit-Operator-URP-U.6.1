using UnityEngine.UIElements;
using UnityEngine;

namespace ATC.Operator.MapView {
    
    public class Map_Node_Draggable {
        internal Rect localBound { get { return draggableRoot.localBound; } }


        [Header("Root Element")]
        private VisualElement parentContainer;

        // The specific VisualElement that will be draggable
        private VisualElement draggableRoot;
        private Vector3 _tmp_mousePos;
        private Vector3 _tmp_dragPos;

        [Header("Offset from Airplane Icon")]
        private Vector2 draggableOffset = new Vector2(-200f, -200f);
        private Vector2 lastAirplanePos;

        private bool _isDragging;

        [Header("Clamping Position")]
        private float minX;
        private float maxX;
        private float minY;
        private float maxY;
        private const float margin = 20f;

        private Map_Node mapNode;

        internal void Initialize(Map_Node rMapNode, VisualElement rMapNode_cloneElement, VisualElement rParentContainer) {
            mapNode = rMapNode;

            // find root and parent
            parentContainer = rParentContainer;
            draggableRoot = rMapNode_cloneElement.Q<VisualElement>("draggableRoot");

            // Register event callbacks for drag functionality
            draggableRoot.RegisterCallback<PointerDownEvent>(OnPointerDown);
            draggableRoot.RegisterCallback<PointerMoveEvent>(OnPointerMove);
            draggableRoot.RegisterCallback<PointerUpEvent>(OnPointerUp);
            draggableRoot.RegisterCallback<GeometryChangedEvent>(OnGemetryChange);

            // Generate Random Direction Offset 
            Vector2 rndDirection = new Vector2(Random.value, Random.value);
            draggableOffset = rndDirection.normalized * 200;

            // Calculate bound
            CalculateMinMaxBound();
        }









        private void OnPointerDown(PointerDownEvent evt) {
            // Only initiate drag with the left mouse button
            if (evt.button == (int)MouseButton.LeftMouse) {
                _isDragging = true;
                // Capture pointer to receive move events even if cursor leaves the element
                draggableRoot.CapturePointer(evt.pointerId);
                // record mouse positoin
                _tmp_mousePos = evt.position;

                // Store the element's position relative to its parent at the start of the drag
                _tmp_dragPos = new Vector2(draggableRoot.layout.x, draggableRoot.layout.y);

                // Calculate bound
                CalculateMinMaxBound();
            }
            // Stop event propagation to prevent it from bubbling up to parent elements
            evt.StopPropagation();
        }


        private void OnPointerMove(PointerMoveEvent evt) {
            // Proceed only if dragging is active and the element has pointer capture
            if (_isDragging && draggableRoot.HasPointerCapture(evt.pointerId)) {
                // Calculate how much the mouse has moved since the drag started
                Vector2 mouseDelta = evt.position - _tmp_mousePos;

                // set and clamp position based on mouse delta
                SetClampPosition(_tmp_dragPos.x + mouseDelta.x, _tmp_dragPos.y + mouseDelta.y);
                // update the connected line as well
               // mapNode.OnGeometryChange_Draggable();
            }
            evt.StopPropagation();
        }


        private void OnPointerUp(PointerUpEvent evt) {
            // Release drag state and pointer capture if active
            if (_isDragging && draggableRoot.HasPointerCapture(evt.pointerId)) {
                _isDragging = false;
                draggableRoot.ReleasePointer(evt.pointerId);
                RecordDraggableOffset();
            }
            evt.StopPropagation();
        }









        private void OnGemetryChange(GeometryChangedEvent evt) {   
            mapNode.OnGeometryChange_Draggable();
        }

        private void CalculateMinMaxBound() {
            // Get the parent container's dimensions
            Rect parentRect = parentContainer.layout;
            float parentWidth = parentRect.width;
            float parentHeight = parentRect.height;

            // Get the draggable element's current dimensions
            float elementWidth = draggableRoot.layout.width;
            float elementHeight = draggableRoot.layout.height;

            // Calculate the min/max allowed positions for the element's top-left corner
            // ensuring the entire element stays within the parent with the defined margin.
            minX = margin;
            maxX = parentWidth - elementWidth - margin;

            minY = margin;
            maxY = parentHeight - elementHeight - margin;
        }








        private void RecordDraggableOffset() {
            draggableOffset.x = draggableRoot.layout.x - lastAirplanePos.x;
            draggableOffset.y = draggableRoot.layout.y - lastAirplanePos.y;
        }
        private void SetClampPosition(float xPos, float yPos) {
            draggableRoot.style.left = Mathf.Clamp(xPos, minX, maxX);
            draggableRoot.style.top = Mathf.Clamp(yPos, minY, maxY);
        }




        internal void OnAirplanePositionUpdate(Vector2 _airplaneCenterPosition) {
            lastAirplanePos = _airplaneCenterPosition;
            if (!_isDragging) {
                SetClampPosition(lastAirplanePos.x + draggableOffset.x, lastAirplanePos.y + draggableOffset.y);
            }
        }

    }
}