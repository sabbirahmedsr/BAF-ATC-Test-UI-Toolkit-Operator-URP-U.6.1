// File: Assets/Scripts/Utilities/RectUtils.cs
using UnityEngine;

namespace ATC.Operator.MapView {
    public static class RectUtils {
        /// <summary>
        /// Finds the intersection point of a line originating from the center of a rectangle
        /// and extending towards a target point, with the rectangle's perimeter.
        /// Assumes the target point is outside or on the perimeter of the rectangle.
        /// </summary>
        /// <param name="rect">The rectangle's world bounds (e.g., element.worldBound).</param>
        /// <param name="targetPoint">The point towards which the line extends (e.g., otherElement.worldBound.center).</param>
        /// <returns>The intersection point on the rect's edge, or the center of the rect if calculation is invalid (e.g., direction is zero).</returns>
        public static Vector2 GetEdgePoint(Rect rect, Vector2 targetPoint) {
            Vector2 center = rect.center;
            Vector2 direction = targetPoint - center;

            // If targetPoint is exactly at center, or very close, return center to avoid division by zero or weird results.
            if (direction.magnitude < 0.001f) // Use a small epsilon for float comparison
            {
                return center;
            }

            // Normalize the direction for consistent scaling
            direction = direction.normalized;

            float minT = float.MaxValue;
            Vector2 intersection = center; // Default to center if no intersection found (shouldn't happen if target is outside)

            // Iterate through each of the four side lines of the rectangle.
            // We are looking for the smallest positive 't' value where P = center + t * direction
            // intersects one of the rectangle's edges.

            // Left edge (x = rect.xMin)
            if (direction.x < 0) // Only if moving left
            {
                float t = (rect.xMin - center.x) / direction.x;
                if (t >= 0 && t < minT) // Check for forward intersection and smallest t
                {
                    Vector2 p = center + t * direction;
                    // Verify that the intersection point is actually on the segment of the edge
                    if (p.y >= rect.yMin && p.y <= rect.yMax) {
                        minT = t;
                        intersection = p;
                    }
                }
            }

            // Right edge (x = rect.xMax)
            if (direction.x > 0) // Only if moving right
            {
                float t = (rect.xMax - center.x) / direction.x;
                if (t >= 0 && t < minT) {
                    Vector2 p = center + t * direction;
                    if (p.y >= rect.yMin && p.y <= rect.yMax) {
                        minT = t;
                        intersection = p;
                    }
                }
            }

            // Bottom edge (y = rect.yMin)
            if (direction.y < 0) // Only if moving down
            {
                float t = (rect.yMin - center.y) / direction.y;
                if (t >= 0 && t < minT) {
                    Vector2 p = center + t * direction;
                    if (p.x >= rect.xMin && p.x <= rect.xMax) {
                        minT = t;
                        intersection = p;
                    }
                }
            }

            // Top edge (y = rect.yMax)
            if (direction.y > 0) // Only if moving up
            {
                float t = (rect.yMax - center.y) / direction.y;
                if (t >= 0 && t < minT) {
                    Vector2 p = center + t * direction;
                    if (p.x >= rect.xMin && p.x <= rect.xMax) {
                        minT = t;
                        intersection = p;
                    }
                }
            }

            return intersection;
        }
    }
}