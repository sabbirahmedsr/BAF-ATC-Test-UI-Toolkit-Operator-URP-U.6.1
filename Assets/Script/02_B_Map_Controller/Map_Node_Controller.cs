using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UIElements;

namespace ATC.Operator.MapView {
    [System.Serializable]
    public class Map_Node_Controller {
        [SerializeField] internal VisualTreeAsset mapNodeTemplate;
        internal VisualElement mapNodeContainer;

        private List<Map_Node> activeMapNode = new List<Map_Node>();
        private List<Map_Node> mapNodePool = new List<Map_Node>();

        internal void Initialize(Map_Controller _mapController) {
            mapNodeContainer = _mapController.mapRoot.Q<VisualElement>("mapNodeContainer");
            Map_Node newMapNode = new Map_Node(this);
            Debug.Log(newMapNode.vizElement.name);
        }
    }
}