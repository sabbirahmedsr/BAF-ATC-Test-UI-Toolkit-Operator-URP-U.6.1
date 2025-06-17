using UnityEngine;

namespace ATC.Operator.MapView {
    public enum RadarMapType { _50nm = 0, _25nm = 1, _12nm = 3 }
    public class RadarMapController : ATC_Operator_Sub_Controller {
        internal RadarMapType mapType;
        [SerializeField] private Map_View_Controller mapViewController;

        internal override void Initialize(ATC_Operator_Main_Controller _mainController) {
            base.Initialize(_mainController);
            mapViewController.Initialize(mainController.uiDocument);
        }

    }
}
