using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace ATC.Operator.CommandView {
    public class Command_Parameter_Data {

        internal CommandParameter GetArrCmdParameter(ArrParameterID rParameterID, AirplaneData apData) {
            ArrParameterID arrParameterID = rParameterID;
            CommandParameter cmdParameter = new CommandParameter();
            cmdParameter.arrParameterID = arrParameterID;
            //
            if (arrParameterID == ArrParameterID.altitude) {
                cmdParameter.caption = "Altitude";
                cmdParameter.dropDownOptions = System.Enum.GetNames(typeof(Altitude));
            } else if (arrParameterID == ArrParameterID.qnh) {
                cmdParameter.caption = "QNH";
                cmdParameter.dropDownOptions = System.Enum.GetNames(typeof(QNH));
            } else if (arrParameterID == ArrParameterID.arrivalApproach) {
                cmdParameter.caption = "Arrival Approach";
                cmdParameter.dropDownOptions = new string[apData.allArrivalApproach.Length];
                for (int i = 0; i < apData.allArrivalApproach.Length; i++) {
                    cmdParameter.dropDownOptions[i] = apData.allArrivalApproach[i].ToString();
                }
            } else if (arrParameterID == ArrParameterID.eatTime) {
                cmdParameter.caption = "EAT";
                cmdParameter.dropDownOptions = new string[20];
                for (int i = 0; i < 20; i++) {
                    cmdParameter.dropDownOptions[i] = ATCTime.GetFutureClockTimeInHHMM(i * 60);
                }
                cmdParameter.dropDownOptions[0] = "No Delay";
            } else if (arrParameterID == ArrParameterID.arrivalTaxiway) {
                cmdParameter.caption = "Arrival Taxiway";
                cmdParameter.dropDownOptions = new string[apData.allArrTaxiwayID.Length];
                for (int i = 0; i < apData.allArrTaxiwayID.Length; i++) {
                    cmdParameter.dropDownOptions[i] = apData.allArrTaxiwayID[i].ToString();
                }
            } else if (arrParameterID == ArrParameterID.viaTaxiway) {
                cmdParameter.caption = "Via Taxiway";
                cmdParameter.dropDownOptions = System.Enum.GetNames(typeof(ViaTaxiwayID));
            } else if (arrParameterID == ArrParameterID.parkingStand) {
                cmdParameter.caption = "Parking Stand";
                cmdParameter.dropDownOptions = new string[] { "None" };
            }
            //
            return cmdParameter;
        }



        internal CommandParameter GetDepCmdParameter(DepParameterID rParameterID) {
            DepParameterID depParameterID = rParameterID;
            CommandParameter cmdParameter = new CommandParameter();
            cmdParameter.depParameterID = depParameterID;
            //
            if (depParameterID == DepParameterID.altitude) {
                cmdParameter.caption = "Altitude";
                cmdParameter.dropDownOptions = System.Enum.GetNames(typeof(Altitude));
            } else if (depParameterID == DepParameterID.qnh) {
                cmdParameter.caption = "QNH";
                cmdParameter.dropDownOptions = System.Enum.GetNames(typeof(QNH));
            } else if (depParameterID == DepParameterID.time) {
                cmdParameter.caption   = "Time";
                cmdParameter.dropDownOptions = new string[20];
                for (int i = 0; i < 20; i++) {
                    cmdParameter.dropDownOptions[i] = ATCTime.GetFutureClockTimeInHHMM((-2 + i) * 60);
                }
                cmdParameter.valueIndex = 2;
            } else if (depParameterID == DepParameterID.pushbackFacingNS) {
                cmdParameter.caption = "Pushback Facing";
                cmdParameter.dropDownOptions = new string[2] { "North", "South" };
            } else if (depParameterID == DepParameterID.viaTaxiway) {
                cmdParameter.caption = "Via Taxiway";
                cmdParameter.dropDownOptions = System.Enum.GetNames(typeof(ViaTaxiwayID));
            }
            //
            return cmdParameter;
        }
    }
}