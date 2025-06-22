

namespace ATC.Operator.CommandView {
    [System.Serializable]
    internal struct CommandParameter {
        internal ArrParameterID arrParameterID;
        internal DepParameterID depParameterID;
        internal string caption;
        internal string[] drdOptions;
        internal int valueIndex;
    }
}