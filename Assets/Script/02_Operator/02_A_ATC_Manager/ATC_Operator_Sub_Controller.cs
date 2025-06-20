using UnityEngine;

namespace ATC.Operator {
    public class ATC_Operator_Sub_Controller : MonoBehaviour {

        internal ATC_Operator_Main_Controller mainController;

        internal virtual void Initialize(ATC_Operator_Main_Controller _mainController) {
            mainController = _mainController;
        }
    }
}