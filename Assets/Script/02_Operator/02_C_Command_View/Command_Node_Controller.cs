using ATC.Operator.Airplane;
using UnityEngine;
using UnityEngine.UIElements;

namespace ATC.Operator.CommandView {
    public class Command_Node_Controller {

        [SerializeField] internal VisualTreeAsset commandNodeTemplate;
        internal VisualElement commandNodeContainer;

        internal CommandController cmdController;

        internal void Initialize(CommandController rCmdController) {
            cmdController = rCmdController;

            // Find root, and set heading ui
            VisualElement rootElement = rCmdController.mainController.uiDocument.rootVisualElement;
            commandNodeContainer = rootElement.Q<VisualElement>("commandNodeContainer");

            // register call back
            GlobalEvent.onAirplaneCreatedEvent += OnAirplaneCreated;
        }

        private void OnAirplaneCreated(ushort globalID, AirplaneController controller) {

        }

    }
}