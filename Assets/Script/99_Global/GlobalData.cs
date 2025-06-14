
namespace ATC.Global {

    public enum OperatorType { arrival, departure }

    public static class GlobalData {
        internal static OperatorType operatorType = OperatorType.departure;
    }
}