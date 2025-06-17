using UnityEngine;

namespace ATC.Operator.MapView {

    public class SurfaceMapController : ATC_Operator_Sub_Controller {
        [SerializeField] internal Map_View_Controller mapViewController;

        internal override void Initialize(ATC_Operator_Main_Controller _mainController) {
            base.Initialize(_mainController);
            mapViewController.Initialize(mainController.uiDocument);
        }
    }
}
