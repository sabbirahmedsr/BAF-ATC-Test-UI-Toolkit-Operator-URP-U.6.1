using ATC.Operator.Airplane;
using System.Collections.Generic;
using UnityEngine.UIElements;

namespace ATC.Operator.CommandView {

    public class Command_Node_Controller {        

        internal ScrollView scrlCommandNodeContainer;
        internal CommandController cmdController;
        internal CommandNodeReference commandNodeReference = new CommandNodeReference();

        internal void Initialize(CommandController rCmdController) {
            cmdController = rCmdController;
            commandNodeReference.CreateNewList();

            // Find root, and set heading ui
            VisualElement rootElement = rCmdController.mainController.uiDocument.rootVisualElement;
            VisualElement cmdRoot = rootElement.Q<VisualElement>("cmdViewRoot");
            scrlCommandNodeContainer = cmdRoot.Q<ScrollView>("scrlCommandNodeContainer");
          

            // register call back
            GlobalEvent.onAirplaneCreatedEvent += OnAirplaneCreated;
        }

        private void OnAirplaneCreated(ushort globalID, AirplaneController rApController) {
            Command_Node newCmdNode = new Command_Node();
            newCmdNode.Initialize(this, rApController);
            commandNodeReference.activeCommandNode.Add(newCmdNode);
        }
    }

    public struct CommandNodeReference {
        public List<Command_Node> activeCommandNode;
        public List<Command_Node> commandNodePool;

        public void CreateNewList() {
            activeCommandNode = new List<Command_Node>();
            commandNodePool = new List<Command_Node>();
        }
    }
}