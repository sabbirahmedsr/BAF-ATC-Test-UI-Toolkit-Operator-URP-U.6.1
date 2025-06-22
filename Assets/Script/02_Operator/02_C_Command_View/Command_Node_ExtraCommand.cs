using System.Collections.Generic;
using UnityEngine.UIElements;

namespace ATC.Operator.CommandView {
    public class Command_Node_ExtraCommand {
        private VisualElement extraCommandRoot;

        // References to your UI Elements
        private Button btnMore;
        private Button btnExtra01;
        private Button btnExtra02;
        private Button btnExtra03;
        private Button btnExtra04;
        private Button btnExtra05;
        private Button[] allExtraButton;

        private Command_Node cmdNode;
        private bool isExpanded;
        private List<ArrivalCommandID> allExtraCommand = new List<ArrivalCommandID>();

        // Called when the object becomes enabled and active.
        internal void Initialize(Command_Node rCommandNode) {
            cmdNode = rCommandNode;
            var nodeRoot = rCommandNode.cmdNode_cloneElement;

            // get ui reference
            btnMore = nodeRoot.Q<Button>("btnMore");
            btnExtra01 = nodeRoot.Q<Button>("btnExtra01");
            btnExtra02 = nodeRoot.Q<Button>("btnExtra02");
            btnExtra03 = nodeRoot.Q<Button>("btnExtra03");
            btnExtra04 = nodeRoot.Q<Button>("btnExtra04");
            btnExtra05 = nodeRoot.Q<Button>("btnExtra05");
            allExtraButton = new Button[] { btnExtra01, btnExtra02, btnExtra03, btnExtra04, btnExtra05 };

            extraCommandRoot = nodeRoot.Q<VisualElement>("extraCommandRoot");

            // Register callback
            btnMore?.RegisterCallback<ClickEvent>(OnClick_More);
            for (int i = 0; i < allExtraButton.Length; i++) {
                allExtraButton[i].RegisterCallback<ClickEvent>(evt => { OnClick_ExtraButton(i); });
            }
        }

        internal void UnregisterCallback() {
            btnMore?.UnregisterCallback<ClickEvent>(OnClick_More);
            for (int i = 0; i < allExtraButton.Length; i++) {
                allExtraButton[i].UnregisterCallback<ClickEvent>(evt => { OnClick_ExtraButton(i); });
            }
        }



        private void OnClick_More(ClickEvent evt) {
            if (allExtraCommand.Count > 0) return;
            isExpanded = !isExpanded;
            ToggleExtraCommand(isExpanded);
        }


        private void OnClick_ExtraButton(int index) {
            if (index < allExtraCommand.Count) {
                
            }
        }


        private void ToggleExtraCommand(bool rBool) {
            isExpanded = rBool;
            extraCommandRoot.style.display = rBool ? DisplayStyle.Flex : DisplayStyle.None;
        }






        internal void OnUpdateArrivalCommandList(ArrivalCommandID[] rAllArrCmdID) {
            // try to make a list of all extra command
            allExtraCommand.Clear();
            for (int i = 0; i < rAllArrCmdID.Length; i++) {
                if ((ushort)rAllArrCmdID[i] >= 99) {
                    allExtraCommand.Add(rAllArrCmdID[i]);
                }
            }
            
            // setup button variable accordingly
            if (allExtraCommand.Count > 0) {
                for (int i = 0; i < allExtraCommand.Count; i++) {
                    ArrivalCommandID arrCmdId = allExtraCommand[i];
                    if (cmdNode.cmdNodeController.cmdController.arrCommandData.TryGetCommand(arrCmdId, out ArrivalCommand oArrCmd)) {
                        allExtraButton[i].text = oArrCmd.commandName;
                    }
                }
            }

            // hide unncessary button
            for (int i = 0; i < allExtraButton.Length; i++) {
                allExtraButton[i].style.display = i < allExtraCommand.Count ? DisplayStyle.Flex : DisplayStyle.None;
            }

            // hide me
            ToggleExtraCommand(false);
        }




    }
}