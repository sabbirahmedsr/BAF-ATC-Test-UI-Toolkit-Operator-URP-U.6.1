

namespace ATC.Operator.CommandView {
    internal static class Command_Parameter_Result {

        internal static ushort GetArrivalResult(this CommandParameter cmdParameter, Command_Parameter_Window rCmdParameterWindow) {
            ushort result = 0;
            if (cmdParameter.arrParameterID == ArrParameterID.altitude) {
                Altitude altitude = (Altitude)cmdParameter.valueIndex;
                result = (ushort)altitude;
            } else if (cmdParameter.arrParameterID == ArrParameterID.qnh) {
                QNH qnh = (QNH)cmdParameter.valueIndex;
                result = (ushort)qnh;
            } else if (cmdParameter.arrParameterID == ArrParameterID.arrivalApproach) {
                ArrivalApproach arrApproach = rCmdParameterWindow.apController.myData.allArrivalApproach[cmdParameter.valueIndex];
                result = (ushort)arrApproach;
            } else if (cmdParameter.arrParameterID == ArrParameterID.eatTime) {
                if (ushort.TryParse(cmdParameter.dropDownOptions[cmdParameter.valueIndex], out ushort eatTime)) {
                    result = eatTime;
                } else {
                    result = ushort.Parse(ATCTime.GetClockTimeInHHMM());
                }
            } else if (cmdParameter.arrParameterID == ArrParameterID.arrivalTaxiway) {
                ArrTaxiwayID arrTaxiwayID = rCmdParameterWindow.apController.myData.allArrTaxiwayID[cmdParameter.valueIndex];
                result = (ushort)arrTaxiwayID;
            } else if (cmdParameter.arrParameterID == ArrParameterID.viaTaxiway) {
                ViaTaxiwayID viaTaxiwayID = (ViaTaxiwayID)cmdParameter.valueIndex;
                result = (ushort)viaTaxiwayID;
            } else if (cmdParameter.arrParameterID == ArrParameterID.parkingStand) {
                ParkingStandID parkingStandID = rCmdParameterWindow.resultParkingStandID[cmdParameter.valueIndex];
                result = (ushort)parkingStandID;
            }

            return result;
        }
    }
}