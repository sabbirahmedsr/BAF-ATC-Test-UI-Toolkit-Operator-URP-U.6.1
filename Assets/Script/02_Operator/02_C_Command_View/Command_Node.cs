using ATC.Operator.Airplane;
using UnityEngine;
using UnityEngine.UIElements;

namespace ATC.Operator.CommandView {
    public class Command_Node {
        
        internal VisualElement cmdNode_cloneElement;

        [Header("Child Sciprt")]
        private Command_Node_MainCommand mainCommand = new Command_Node_MainCommand();   /// for call sign, main action command, and repeat last call button
        private Command_Node_FixedCommand fixedCommand = new Command_Node_FixedCommand(); /// for highlight, freeze, resume, and auto complete button
        private Command_Node_ExtraCommand extraCommand = new Command_Node_ExtraCommand(); /// for extra command button

        internal Command_Node_Controller cmdNodeController;
        internal AirplaneController apController;
        internal Command_Parameter_Controller cmdParamCtrl;

        // Called when the object becomes enabled and active.
        internal void Initialize(Command_Node_Controller rCmdNodeController, AirplaneController rAP__Controller) {
            cmdNodeController = rCmdNodeController;
            apController = rAP__Controller;
            cmdParamCtrl = cmdNodeController.cmdController.cmdParamController;

            CloneAndProcess_CommandNodeAsync(cmdNodeController.commandNodeTemplate, cmdNodeController.scrlCommandNodeContainer);
        }


        private async void CloneAndProcess_CommandNodeAsync(VisualTreeAsset rCmdNodeTemplate, ScrollView rParentScrollContainer) {
            // Create a instance from map node template and find root element
            cmdNode_cloneElement = rCmdNodeTemplate.CloneTree();

            // Add created instance for cloning in scene view
            rParentScrollContainer.Add(cmdNode_cloneElement);

            // initialize child script
            mainCommand.Initialize(this);
            fixedCommand.Initialize(this);
            extraCommand.Initialize(this);

            // Wait for the end of the frame, after layout has been calculated
            await Awaitable.EndOfFrameAsync();

            // add listener
            apController.apEvent.onUpdate_ArrivalCommandList.AddListener(OnUpdateArrivalCommandList);
            apController.apEvent.onAirplaneDestroyEvent.AddListener(DestroyMe);

            // refresh variable for first time
            OnUpdateArrivalCommandList(apController.allArrivalCommandID);
        }


        private void UnregisterCallback() {
            mainCommand.UnregisterCallback();
            fixedCommand.UnregisterCallback();
            extraCommand.UnregisterCallback();
        }






        void OnUpdateArrivalCommandList(ArrivalCommandID[] rAllArrCmdID) {
            mainCommand.OnUpdateArrivalCommandList(rAllArrCmdID);   
            extraCommand.OnUpdateArrivalCommandList(rAllArrCmdID);
        }


        internal void DestroyMe() {
            //Destroy(gameObject);
        }
    }
}