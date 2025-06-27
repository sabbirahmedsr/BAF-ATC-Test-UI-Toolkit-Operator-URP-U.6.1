using UnityEngine;
using UnityEngine.UIElements;

namespace ATC.Operator.CommandView {
    public class CommandController : ATC_Operator_Sub_Controller {
        [Header("Data")]
        [SerializeField] internal ArrivalCommandData arrCommandData;
        [SerializeField] internal DepartureCommandData depCommandData;
        [SerializeField] internal ParkingPathData parkingPathData;
        [SerializeField] internal AirplaneData[] allAirplaneData;
        
        [Header("UI Template")]
        [SerializeField] internal VisualTreeAsset commandNodeTemplate;

        [Header("Child Script")]
        internal Command_Node_Controller cmdNodeController = new Command_Node_Controller();
        internal Command_Parameter_Controller cmdParamController = new Command_Parameter_Controller();

        internal override void Initialize(ATC_Operator_Main_Controller _mainController) {
            base.Initialize(_mainController);

            cmdNodeController.Initialize(this);
            cmdParamController.Initialize(this);
        }
    }
}