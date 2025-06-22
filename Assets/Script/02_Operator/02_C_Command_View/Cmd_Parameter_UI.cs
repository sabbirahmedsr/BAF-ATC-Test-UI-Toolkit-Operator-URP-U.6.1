using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace ATC.Operator.CommandView {
    [System.Serializable]
    public class Command_Parameter_UI{
        [Header("Root Variable")]
        private VisualElement myRoot;

        private Label txtCallSign;
        private Label txtCommandName;

        private DropdownField drd01;
        private DropdownField drd02;
        private DropdownField drd03;
        private DropdownField drd04;
        private DropdownField[] allDropDown;

        private Button btnConfirm;
        private Button btnCancel;

        private Command_Parameter_Controller cmdParamCtrl;

        internal void Initialize(Command_Parameter_Controller rCmdParamCtrl) {
            cmdParamCtrl = rCmdParamCtrl;

            // Find root 
            VisualElement rootElement = rCmdParamCtrl.cmdController.mainController.uiDocument.rootVisualElement;
            myRoot = rootElement.Q<VisualElement>("cmdParameterWindow");

            // get label reference ui reference
            txtCallSign = myRoot.Q<Label>("txtCallSign");
            txtCommandName = myRoot.Q<Label>("txtCommandName");

            // get dropdown reference
            drd01 = myRoot.Q<DropdownField>("drd01");
            drd02 = myRoot.Q<DropdownField>("drd02");
            drd03 = myRoot.Q<DropdownField>("drd03");
            drd04 = myRoot.Q<DropdownField>("drd04");
            allDropDown = new DropdownField[] { drd01, drd02, drd03, drd04 };

            btnConfirm = myRoot.Q<Button>("btnConfirm");
            btnCancel = myRoot.Q<Button>("btnCancel");

            // register callback
            for (int i = 0; i < allDropDown.Length; i++) {
                int index = i;
                allDropDown[i].RegisterValueChangedCallback<string>(evt => { 
                    rCmdParamCtrl.OnDrdValueChange(index, allDropDown[index].index); 
                });
            }
            btnConfirm.RegisterCallback<ClickEvent>(ConfirmCommand);
            btnCancel.RegisterCallback<ClickEvent>(CancelCommand);
        }

        internal void Refresh_CallSignAndCmdName(ArrivalCommand rArrCmd) {
            txtCallSign.text  = cmdParamCtrl.apController.callSign.ToString();
            txtCommandName.text = rArrCmd.commandName;  
        }


        internal void Refresh_DropdownByParameter() {
            CommandParameter[] allCmdParam = cmdParamCtrl.allCmdParameter;
            // set dropdown according to command parameter
            for (int i = 0; i < allCmdParam.Length; i++) {
                allDropDown[i].label = allCmdParam[i].caption;
                allDropDown[i].choices = allCmdParam[i].drdOptions.ToList();
                int givenValueIndex = allCmdParam[i].valueIndex;
                allDropDown[i].SetValueWithoutNotify(allCmdParam[i].drdOptions[givenValueIndex]);
            }

            // enable required dropdown, disable extra one
            for (int i = 0; i < allDropDown.Length; i++) {
                bool shoudDisplay = i < allCmdParam.Length;
                allDropDown[i].style.display = shoudDisplay ? DisplayStyle.Flex : DisplayStyle.None;
            }
        }

        internal void Activate(bool rBool) {
            myRoot.style.display = rBool? DisplayStyle.Flex : DisplayStyle.None;
        }

        private void CancelCommand(ClickEvent evt) {
            myRoot.style.display = DisplayStyle.None;
        }

        internal void Refresh_ParkingStandDropdown(ViaTaxiwayID rViaTaxiwayID) {
            for (int i = 0; i < cmdParamCtrl.allCmdParameter.Length; i++) {
                if (cmdParamCtrl.allCmdParameter[i].arrParameterID == ArrParameterID.parkingStand) {
                    cmdParamCtrl.resultParkingStandID = cmdParamCtrl.cmdController.parkingPathData.GetArrivalParkingStandList(rViaTaxiwayID);
                    string[] allAvailableParkingStand = new string[cmdParamCtrl.resultParkingStandID.Length];
                    for (int j = 0; j < allAvailableParkingStand.Length; j++) {
                        allAvailableParkingStand[j] = cmdParamCtrl.resultParkingStandID[j].ToString().ToUpper();
                    }
                    cmdParamCtrl.allCmdParameter[i].drdOptions = allAvailableParkingStand;
                  //  allDropDown[i].choices = allAvailableParkingStand.ToList();
                }
            }
        }



        private void ConfirmCommand(ClickEvent evt) {
            cmdParamCtrl.SendCommandToTargetAirplane();
            myRoot.style.display = DisplayStyle.None;
        }

    }
}