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
        private List<ArrivalCommandID> allExtraArrCommand = new List<ArrivalCommandID>();
        private List<DepartureCommandID> allExtraDepCommand = new List<DepartureCommandID>();

        // Called when the object first time created
        internal void Initialize(Command_Node rCommandNode) {
            cmdNode = rCommandNode;

            // get ui reference
            FinUIReference(rCommandNode.cmdNode_cloneElement);
            RegisterCallBack();
        }




        private void FinUIReference(VisualElement nodeRoot) {
            btnMore = nodeRoot.Q<Button>("btnMore");
            btnExtra01 = nodeRoot.Q<Button>("btnExtra01");
            btnExtra02 = nodeRoot.Q<Button>("btnExtra02");
            btnExtra03 = nodeRoot.Q<Button>("btnExtra03");
            btnExtra04 = nodeRoot.Q<Button>("btnExtra04");
            btnExtra05 = nodeRoot.Q<Button>("btnExtra05");
            allExtraButton = new Button[] { btnExtra01, btnExtra02, btnExtra03, btnExtra04, btnExtra05 };
            extraCommandRoot = nodeRoot.Q<VisualElement>("extraCommandRoot");
        }
        private void RegisterCallBack() {
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
            if (allExtraArrCommand.Count <= 0) return;
            isExpanded = !isExpanded;
            ToggleExtraCommand(isExpanded);
        }
        private void ToggleExtraCommand(bool rBool) {
            isExpanded = rBool;
            extraCommandRoot.style.display = rBool ? DisplayStyle.Flex : DisplayStyle.None;
        }





        private void OnClick_ExtraButton(int index) {
            if (index < allExtraArrCommand.Count) {
                
            }
        }








        internal void OnUpdate_ArrivalCommandList(ArrivalCommandID[] rAllArrCmdID) {
            // try to make a list of all extra command
            allExtraArrCommand.Clear();
            for (int i = 0; i < rAllArrCmdID.Length; i++) {
                if ((ushort)rAllArrCmdID[i] >= 99) {
                    allExtraArrCommand.Add(rAllArrCmdID[i]);
                }
            }
            
            // setup button variable accordingly
            if (allExtraArrCommand.Count > 0) {
                for (int i = 0; i < allExtraArrCommand.Count; i++) {
                    ArrivalCommandID arrCmdId = allExtraArrCommand[i];
                    if (cmdNode.cmdNodeCtrl.cmdController.arrCommandData.TryGetCommand(arrCmdId, out ArrivalCommand oArrCmd)) {
                        allExtraButton[i].text = oArrCmd.commandName;
                    }
                }
            }

            // hide unncessary button
            for (int i = 0; i < allExtraButton.Length; i++) {
                allExtraButton[i].style.display = i < allExtraArrCommand.Count ? DisplayStyle.Flex : DisplayStyle.None;
            }
            btnMore.SetEnabled(allExtraArrCommand.Count > 0);

            // hide me
            ToggleExtraCommand(false);
        }



        internal void OnUpdate_DepartureCommandList(DepartureCommandID[] rAllDepCmdID) {
            // try to make a list of all extra command
            allExtraDepCommand.Clear();
            for (int i = 0; i < rAllDepCmdID.Length; i++) {
                if ((ushort)rAllDepCmdID[i] >= 99) {
                    allExtraDepCommand.Add(rAllDepCmdID[i]);
                }
            }

            // setup button variable accordingly
            if (allExtraDepCommand.Count > 0) {
                for (int i = 0; i < allExtraDepCommand.Count; i++) {
                    DepartureCommandID depCmdId = allExtraDepCommand[i];
                    if (cmdNode.cmdNodeCtrl.cmdController.depCommandData.TryGetCommand(depCmdId, out DepartureCommand oDepCmd)) {
                        allExtraButton[i].text = oDepCmd.commandName;
                    }
                }
            }

            // hide unncessary button
            for (int i = 0; i < allExtraButton.Length; i++) {
                allExtraButton[i].style.display = i < allExtraDepCommand.Count ? DisplayStyle.Flex : DisplayStyle.None;
            }
            btnMore.SetEnabled(allExtraDepCommand.Count > 0);

            // hide me
            ToggleExtraCommand(false);
        }
        



    }
}