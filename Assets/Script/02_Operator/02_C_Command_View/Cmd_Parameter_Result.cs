

namespace ATC.Operator.CommandView {
    internal static class Cmd_Parameter_Result {

        internal static ushort GetArrivalResult(this CommandParameter cmdParameter, Command_Parameter_Controller rCmdParamCtrl) {
            ushort result = 0;
            if (cmdParameter.arrParameterID == ArrParameterID.altitude) {
                Altitude altitude = (Altitude)cmdParameter.valueIndex;
                result = (ushort)altitude;
            } else if (cmdParameter.arrParameterID == ArrParameterID.qnh) {
                QNH qnh = (QNH)cmdParameter.valueIndex;
                result = (ushort)qnh;
            } else if (cmdParameter.arrParameterID == ArrParameterID.arrivalApproach) {
                ArrivalApproach arrApproach = rCmdParamCtrl.apController.myData.allArrivalApproach[cmdParameter.valueIndex];
                result = (ushort)arrApproach;
            } else if (cmdParameter.arrParameterID == ArrParameterID.eatTime) {
                if (ushort.TryParse(cmdParameter.drdOptions[cmdParameter.valueIndex], out ushort eatTime)) {
                    result = eatTime;
                } else {
                    result = ushort.Parse(ATCTime.GetClockTimeInHHMM());
                }
            } else if (cmdParameter.arrParameterID == ArrParameterID.arrivalTaxiway) {
                ArrTaxiwayID arrTaxiwayID = rCmdParamCtrl.apController.myData.allArrTaxiwayID[cmdParameter.valueIndex];
                result = (ushort)arrTaxiwayID;
            } else if (cmdParameter.arrParameterID == ArrParameterID.viaTaxiway) {
                ViaTaxiwayID viaTaxiwayID = (ViaTaxiwayID)cmdParameter.valueIndex;
                result = (ushort)viaTaxiwayID;
            } else if (cmdParameter.arrParameterID == ArrParameterID.parkingStand) {
                ParkingStandID parkingStandID = rCmdParamCtrl.resultParkingStandID[cmdParameter.valueIndex];
                result = (ushort)parkingStandID;
            }

            return result;
        }
    }
}