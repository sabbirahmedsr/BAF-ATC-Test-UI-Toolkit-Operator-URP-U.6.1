using UnityEngine;
using UnityEngine.UIElements;
namespace ATC.Operator.MapView {

    public class Map_Node_AirplaneIcon {
        internal Rect localBound { get { return apIconRoot.localBound; } }

        private VisualElement apIconRoot;
        private Vector2 headingOffset = new Vector2(0, -40);   /// to exclude the heading height from container

        private Map_Node mapNode;
        internal void Initialize(Map_Node rMapNode, VisualElement rMapNode_CloneElement) {
            this.mapNode = rMapNode;
            apIconRoot = rMapNode_CloneElement.Q<VisualElement>("airplaneIcon");

            apIconRoot.RegisterCallback<GeometryChangedEvent>(OnGeometryChange);
        }

        internal void OnAirplanePositionUpdate(Vector2 screenPosition) {
            screenPosition -= new Vector2(apIconRoot.layout.width / 2f, apIconRoot.layout.height / 2f);
            screenPosition += headingOffset;

            apIconRoot.style.left = screenPosition.x;
            apIconRoot.style.top = screenPosition.y;
        }

        private void OnGeometryChange(GeometryChangedEvent evt) {
            mapNode.OnGeometryChange_AirplaneIcons();
        }
    }
}