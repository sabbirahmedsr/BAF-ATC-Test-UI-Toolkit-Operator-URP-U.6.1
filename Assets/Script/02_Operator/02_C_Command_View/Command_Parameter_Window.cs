using ATC.Global;
using ATC.Operator.Airplane;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace ATC.Operator.CommandView {
    [System.Serializable]
    public class Command_Parameter_Window {

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

        private CommandParameter[] allCommandParameter;
        private Command_Parameter_Data cmdParameterData;

        internal CommandController cmdController;
        internal AirplaneController apController;

        ArrivalCommandID arrCommandID;
        internal ParkingStandID[] resultParkingStandID = new ParkingStandID[] { };

        

        internal void Initialize(CommandController rCmdController) {
            cmdController = rCmdController;

            // Find root 
            VisualElement rootElement = rCmdController.mainController.uiDocument.rootVisualElement;
            VisualElement myRoot = rootElement.Q<VisualElement>("cmdParameterWindow");

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
                allDropDown[i].RegisterValueChangedCallback<string>(evt => { OnDrdValueChange(i, allDropDown[i].index); });
            }
            btnConfirm.RegisterCallback<ClickEvent>(ConfirmCommand);
            btnCancel.RegisterCallback<ClickEvent>(CancelCommand);
        }

        internal void Activate(AirplaneController rAPController, ArrivalCommandID rArrCmdID) {
            arrCommandID = rArrCmdID;
            apController = rAPController;
            // try to get command from data
            if (cmdController.arrCommandData.TryGetCommand(rArrCmdID, out ArrivalCommand arrCommand)) {
                // set call sign and command name
                txtCallSign.text = apController.callSign.ToString();
                if(txtCommandName == null) {
                    Debug.Log("Error Here");
                }
                txtCommandName.text = arrCommand.commandName;

                // get parameter list
                allCommandParameter = new CommandParameter[arrCommand.allParameterID.Length];
                for (int i = 0; i < arrCommand.allParameterID.Length; i++) {
                    CommandParameter cmdParameter = cmdParameterData.GetArrCmdParameter(arrCommand.allParameterID[i], rAPController.myData);
                    allCommandParameter[i] = cmdParameter;
                }

                // set dropdown according to command parameter
                for (int i = 0; i < allCommandParameter.Length; i++) {
                    allDropDown[i].label = allCommandParameter[i].caption;
                    allDropDown[i].choices = allCommandParameter[i].dropDownOptions.ToList();
                    int givenValueIndex = allCommandParameter[i].valueIndex;
                    allDropDown[i].SetValueWithoutNotify(allCommandParameter[i].dropDownOptions[givenValueIndex]);
                }

                // enable required dropdown, disable extra one
                for (int i = 0; i < allDropDown.Length; i++) {
                    bool shoudDisplay = i < allCommandParameter.Length;
                    allDropDown[i].style.display = shoudDisplay ? DisplayStyle.Flex : DisplayStyle.None;
                }

                // if we have giveTaxiToPark, generate the parkign stand based on drop down selection
                if (rArrCmdID == ArrivalCommandID.giveTaxiToPark) {
                    for (int i = 0; i < allCommandParameter.Length; i++) {
                        if (allCommandParameter[i].arrParameterID == ArrParameterID.viaTaxiway) {
                            allCommandParameter[i].valueIndex = (int)ViaTaxiwayID.C;
                        }
                    }
                    RefreshParkingStandDropdown(ViaTaxiwayID.C);
                }
            }
            myRoot.style.display = DisplayStyle.Flex;
        }

        private void CancelCommand(ClickEvent evt) {
            myRoot.style.display = DisplayStyle.None;
        }


        private void OnDrdValueChange(int rDrdSerial, int rDrdValue) {
            allCommandParameter[rDrdSerial].valueIndex = rDrdValue;
            if (allCommandParameter[rDrdSerial].arrParameterID == ArrParameterID.viaTaxiway) {
                RefreshParkingStandDropdown((ViaTaxiwayID)rDrdValue);
            }
        }
        private void RefreshParkingStandDropdown(ViaTaxiwayID rViaTaxiwayID) {
            for (int i = 0; i < allCommandParameter.Length; i++) {
                if (allCommandParameter[i].arrParameterID == ArrParameterID.parkingStand) {
                    resultParkingStandID = cmdController.parkingPathData.GetArrivalParkingStandList(rViaTaxiwayID);
                    string[] allAvailableParkingStand = new string[resultParkingStandID.Length];
                    for (int j = 0; j < allAvailableParkingStand.Length; j++) {
                        allAvailableParkingStand[j] = resultParkingStandID[j].ToString().ToUpper();
                    }
                    allCommandParameter[i].dropDownOptions = allAvailableParkingStand;
                }
            }
        }



        private void ConfirmCommand(ClickEvent evt) {
            SendCommandToTargetAirplane();
            myRoot.style.display = DisplayStyle.None;
        }
        private void SendCommandToTargetAirplane() {
            if (apController == null)
                return;

            ushort[] parameterResult = new ushort[allCommandParameter.Length];
            for (int i = 0; i < parameterResult.Length; i++) {
                parameterResult[i] = allCommandParameter[i].GetArrivalResult(this);
            }

            GlobalNetwork.actionSender.Send_AP_FlightCommand(apController.globalID, (ushort)arrCommandID, parameterResult);
        }

    }
}