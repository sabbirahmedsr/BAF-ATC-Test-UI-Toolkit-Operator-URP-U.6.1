using ATC.Global;
using ATC.Operator.Airplane;
using UnityEngine;

namespace ATC.Operator.CommandView {
    [System.Serializable]
    public class Command_Parameter_Controller {
        [Header("Variable Reference")]
        private ArrivalCommandID arrCommandID;
        internal CommandParameter[] allCmdParameter;
        internal ParkingStandID[] resultParkingStandID = new ParkingStandID[] { };

        [Header("UI and Child Reference")]
        internal Command_Parameter_UI ui = new Command_Parameter_UI();

        [Header("Parent and Airplane Reference")]
        internal CommandController cmdController;
        internal AirplaneController apController;
        

        internal void Initialize(CommandController rCmdController) {
            cmdController = rCmdController;
            ui.Initialize(this);
        }

        internal void Activate(AirplaneController rAPController, ArrivalCommandID rArrCmdID) {
            arrCommandID = rArrCmdID;
            apController = rAPController;
            // try to get command from data
            if (cmdController.arrCommandData.TryGetCommand(rArrCmdID, out ArrivalCommand arrCommand)) {
                ui.Refresh_CallSignAndCmdName(arrCommand);
                
                // get parameter list
                allCmdParameter = new CommandParameter[arrCommand.allParameterID.Length];
                for (int i = 0; i < arrCommand.allParameterID.Length; i++) {
                    CommandParameter cmdParameter = this.GetArrCmdParameter(arrCommand.allParameterID[i]);
                    allCmdParameter[i] = cmdParameter;
                }

                // refresh ui accordingly
                ui.Refresh_DropdownByParameter();

                // if rArrCmdID is giveTaxiToPark, generate the parkign stand based on drop down selection
                if (rArrCmdID == ArrivalCommandID.giveTaxiToPark) {
                    for (int i = 0; i < allCmdParameter.Length; i++) {
                        if (allCmdParameter[i].arrParameterID == ArrParameterID.viaTaxiway) {
                            allCmdParameter[i].valueIndex = (int)ViaTaxiwayID.C;
                        }
                    }
                    ui.Refresh_ParkingStandDropdown(ViaTaxiwayID.C);
                }
            }
            ui.Activate(true);
        }


        internal void OnDrdValueChange(int rDrdSerial, int rDrdValue) {
            allCmdParameter[rDrdSerial].valueIndex = rDrdValue;
            if (allCmdParameter[rDrdSerial].arrParameterID == ArrParameterID.viaTaxiway) {
               ui.Refresh_ParkingStandDropdown((ViaTaxiwayID)rDrdValue);
            }
        }


        internal void SendCommandToTargetAirplane() {
            if (apController == null)
                return;

            ushort[] parameterResult = new ushort[allCmdParameter.Length];
            for (int i = 0; i < parameterResult.Length; i++) {
                parameterResult[i] = allCmdParameter[i].GetArrivalResult(this);
            }

            GlobalNetwork.actionSender.Send_AP_FlightCommand(apController.globalID, (ushort)arrCommandID, parameterResult);
        }

    }
}