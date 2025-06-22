using ATC.Global;
using UnityEngine;
using UnityEngine.UIElements;

namespace ATC.Operator.CommandView {
    public class Command_Node_MainCommand {

        // References to your UI Elements
        private Label txtCallSign;
        private Button btnMainAction;
        private Button btnRepeatLastcall;

        private ArrivalCommandID mainArrCmdID = ArrivalCommandID.none;
        private DepartureCommandID mainDepCmdID = DepartureCommandID.none;

        private Command_Node cmdNode;


        internal void Initialize(Command_Node rCmdNode) {
            cmdNode = rCmdNode;

            var nodeRoot = rCmdNode.cmdNode_cloneElement;

            // get ui reference
            txtCallSign = nodeRoot.Q<Label>("txtCallSign");
            btnMainAction = nodeRoot.Q<Button>("btnMainAction");
            btnRepeatLastcall = nodeRoot.Q<Button>("btnRepeatLastcall");

            // set ui
            txtCallSign.text = rCmdNode.apController.callSign.ToString();

            // Register callback
            btnMainAction?.RegisterCallback<ClickEvent>(OnClick_MainAction);
            btnRepeatLastcall?.RegisterCallback<ClickEvent>(OnClick_RepeatLastCall);
        }

        internal void UnregisterCallback() {
            btnMainAction?.UnregisterCallback<ClickEvent>(OnClick_MainAction);
            btnRepeatLastcall?.UnregisterCallback<ClickEvent>(OnClick_RepeatLastCall);
        }


        private void OnClick_MainAction(ClickEvent evt) {
            ActivateMainArrCommand();
        }

        private void OnClick_RepeatLastCall(ClickEvent evt) {
            Debug.Log("Repeat Command button clicked!");
        }



        private void ActivateMainArrCommand() {
            if (mainArrCmdID != ArrivalCommandID.none) {
                if (cmdNode.cmdNodeController.cmdController.arrCommandData.TryGetCommand(mainArrCmdID, out ArrivalCommand arrCommand)) {
                    if (arrCommand.allParameterID.Length > 0) {
                        cmdNode.cmdParameterWindow.Activate(cmdNode.apController, mainArrCmdID);
                    } else {
                        GlobalNetwork.actionSender.Send_AP_FlightCommand(cmdNode.apController.globalID, (ushort)mainArrCmdID, new ushort[] { });
                    }
                }
            }
        }





        internal void OnUpdateArrivalCommandList(ArrivalCommandID[] rAllArrCmdID) {
            // At first check if there is any main action
            bool hasMainAction = false;
            for (int i = 0; i < rAllArrCmdID.Length; i++) {
                if ((ushort)rAllArrCmdID[i] <= 99) {
                    mainArrCmdID = rAllArrCmdID[i];
                    hasMainAction = true;
                    break;
                }
            }

            // set visibility based on that
            btnMainAction.style.display = hasMainAction ? DisplayStyle.Flex : DisplayStyle.None;
            btnRepeatLastcall.style.display = hasMainAction ? DisplayStyle.None : DisplayStyle.Flex;

            // if we have main action, update the required button name
            if (mainArrCmdID != ArrivalCommandID.none) {
                
                if (cmdNode.cmdNodeController.cmdController.arrCommandData.TryGetCommand(mainArrCmdID, out ArrivalCommand arrCmd)) {
                    btnMainAction.text = arrCmd.commandName;
                }
            }
        }
    }
}