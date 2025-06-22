

namespace ATC.Operator.CommandView {
    public static class Cmd_Parameter_Data {
        internal static CommandParameter GetArrCmdParameter(this Command_Parameter_Controller rCmdParamCtrl, ArrParameterID rParameterID) {
            AirplaneData apData = rCmdParamCtrl.apController.myData;
            ArrParameterID arrParameterID = rParameterID;
            CommandParameter cmdParameter = new CommandParameter();
            cmdParameter.arrParameterID = arrParameterID;
            //
            if (arrParameterID == ArrParameterID.altitude) {
                cmdParameter.caption = "Altitude";
                cmdParameter.drdOptions = System.Enum.GetNames(typeof(Altitude));
            } else if (arrParameterID == ArrParameterID.qnh) {
                cmdParameter.caption = "QNH";
                cmdParameter.drdOptions = System.Enum.GetNames(typeof(QNH));
            } else if (arrParameterID == ArrParameterID.arrivalApproach) {
                cmdParameter.caption = "Arrival Approach";
                cmdParameter.drdOptions = new string[apData.allArrivalApproach.Length];
                for (int i = 0; i < apData.allArrivalApproach.Length; i++) {
                    cmdParameter.drdOptions[i] = apData.allArrivalApproach[i].ToString();
                }
            } else if (arrParameterID == ArrParameterID.eatTime) {
                cmdParameter.caption = "EAT";
                cmdParameter.drdOptions = new string[20];
                for (int i = 0; i < 20; i++) {
                    cmdParameter.drdOptions[i] = ATCTime.GetFutureClockTimeInHHMM(i * 60);
                }
                cmdParameter.drdOptions[0] = "No Delay";
            } else if (arrParameterID == ArrParameterID.arrivalTaxiway) {
                cmdParameter.caption = "Arrival Taxiway";
                cmdParameter.drdOptions = new string[apData.allArrTaxiwayID.Length];
                for (int i = 0; i < apData.allArrTaxiwayID.Length; i++) {
                    cmdParameter.drdOptions[i] = apData.allArrTaxiwayID[i].ToString();
                }
            } else if (arrParameterID == ArrParameterID.viaTaxiway) {
                cmdParameter.caption = "Via Taxiway";
                cmdParameter.drdOptions = System.Enum.GetNames(typeof(ViaTaxiwayID));
            } else if (arrParameterID == ArrParameterID.parkingStand) {
                cmdParameter.caption = "Parking Stand";
                cmdParameter.drdOptions = new string[] { "None" };
            }
            //
            return cmdParameter;
        }



        internal static CommandParameter GetDepCmdParameter(this Command_Parameter_Controller rCmdParamCtrl, DepParameterID rParameterID) {
            DepParameterID depParameterID = rParameterID;
            CommandParameter cmdParameter = new CommandParameter();
            cmdParameter.depParameterID = depParameterID;
            //
            if (depParameterID == DepParameterID.altitude) {
                cmdParameter.caption = "Altitude";
                cmdParameter.drdOptions = System.Enum.GetNames(typeof(Altitude));
            } else if (depParameterID == DepParameterID.qnh) {
                cmdParameter.caption = "QNH";
                cmdParameter.drdOptions = System.Enum.GetNames(typeof(QNH));
            } else if (depParameterID == DepParameterID.time) {
                cmdParameter.caption   = "Time";
                cmdParameter.drdOptions = new string[20];
                for (int i = 0; i < 20; i++) {
                    cmdParameter.drdOptions[i] = ATCTime.GetFutureClockTimeInHHMM((-2 + i) * 60);
                }
                cmdParameter.valueIndex = 2;
            } else if (depParameterID == DepParameterID.pushbackFacingNS) {
                cmdParameter.caption = "Pushback Facing";
                cmdParameter.drdOptions = new string[2] { "North", "South" };
            } else if (depParameterID == DepParameterID.viaTaxiway) {
                cmdParameter.caption = "Via Taxiway";
                cmdParameter.drdOptions = System.Enum.GetNames(typeof(ViaTaxiwayID));
            }
            //
            return cmdParameter;
        }
    }
}