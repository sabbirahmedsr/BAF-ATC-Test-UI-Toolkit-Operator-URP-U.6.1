using UnityEngine;
using UnityEngine.UIElements;

namespace ATC.Operator.MapView {
    public class Map_Node {
        internal TemplateContainer vizElement;
        private Map_Node_Controller mapNodeController;

        public Map_Node(Map_Node_Controller _map_Node_Controller) {
            mapNodeController = _map_Node_Controller;

            vizElement = mapNodeController.mapNodeTemplate.CloneTree();
            mapNodeController.mapNodeContainer.Add(vizElement);

            vizElement.RegisterCallback<PointerDownEvent>(OnPointerDown);
            vizElement.RegisterCallback<PointerUpEvent>(OnPointerUp);
        }

        private void OnPointerDown(PointerDownEvent evt) {
            Debug.Log(evt.position); 
        }
        private void OnPointerUp(PointerUpEvent evt) {
            Debug.Log(evt.position);
        }
    }
}