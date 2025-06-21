using UnityEngine;
using UnityEngine.UIElements;

namespace ATC.Operator.MapView {

    public class Map_Node_Connecting_Line : VisualElement {
        private Vector2 _startPoint;
        private Vector2 _endPoint;
        private Color _color = Color.yellow;
        private Color _capColor = Color.yellow;
        private const float _thickness = 3f;
        private const float _capRadius = 6f;    

        /// <summary>
        /// Constructor for the LineDrawer VisualElement.
        /// </summary>
        /// <param name="start">The starting point of the line in local coordinates.</param>
        /// <param name="end">The ending point of the line in local coordinates.</param>
        /// <param name="thickness">The thickness of the line.</param>
        /// <param name="color">The color of the line.</param>
        /// <param name="capRadius">The radius of the circles at the line ends. Set to 0 for no caps.</param>
        /// <param name="capColor">The color of the circles at the line ends.</param>
        public Map_Node_Connecting_Line(Vector2 start, Vector2 end) {
            _startPoint = start;
            _endPoint = end;

            // Set style to fill the parent and allow drawing anywhere within its bounds.
            // The actual position will be managed by the parent's layout or absolute positioning.
            style.position = Position.Absolute;
            style.left = 0;
            style.top = 0;
            style.right = 0;
            style.bottom = 0;

            // Register the callback that draws the custom content for this VisualElement.
            generateVisualContent += OnGenerateVisualContent;
        }

        /// <summary>
        /// Updates the start and end points of the line and requests a repaint.
        /// </summary>
        /// <param name="start">New start point.</param>
        /// <param name="end">New end point.</param>
        private void SetPoints(Vector2 start, Vector2 end) {
            // Only update and repaint if the points have actually changed to optimize.
            // Using a small epsilon for float comparison to avoid unnecessary repaints due to tiny float differences.
            if (Vector2.Distance(_startPoint, start) > 0.01f || Vector2.Distance(_endPoint, end) > 0.01f) {
                _startPoint = start;
                _endPoint = end;
                MarkDirtyRepaint(); // Essential: tells UI Toolkit to redraw this element
            }
        }

        internal void UpdateConnectingLine(Rect rDraggableRect, Vector2 rAirplaneCenter) {
            Vector2 p0 = RectUtils.GetEdgePoint(rDraggableRect, rAirplaneCenter);
            Vector2 p1 = rAirplaneCenter + (p0 - rAirplaneCenter).normalized * 20;
            SetPoints(p0, p1);
        }

        /// <summary>
        /// The callback method where custom drawing (like lines and circles) occurs using Painter2D.
        /// </summary>
        private void OnGenerateVisualContent(MeshGenerationContext context) {
            Painter2D painter = context.painter2D;

            // --- Draw the Line ---
            painter.lineWidth = _thickness;
            painter.strokeColor = _color;
            painter.BeginPath();
            painter.MoveTo(_startPoint);
            painter.LineTo(_endPoint);
            painter.Stroke();

            // --- Draw the Circles (Caps) 

            painter.fillColor = _capColor; // Use fill color for circles
            painter.strokeColor = _capColor; // Or a different color if you want a border

            // Start Cap Circle
            painter.BeginPath();
            painter.Arc(_startPoint, _capRadius, 0, 360); // Draw full circle
            painter.Fill();

            // End Cap Circle
            painter.BeginPath();
            painter.Arc(_endPoint, _capRadius, 0, 360); // Draw full circle
            painter.Fill();
        }
    }
}