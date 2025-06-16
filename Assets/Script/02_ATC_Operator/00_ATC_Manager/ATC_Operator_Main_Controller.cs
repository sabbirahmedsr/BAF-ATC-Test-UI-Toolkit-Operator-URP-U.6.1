using UnityEngine;
using UnityEngine.UIElements;
using ATC.Operator.MapView;

namespace ATC.Operator {
    public class ATC_Operator_Main_Controller : MonoBehaviour {
        [SerializeField] internal UIDocument uiDocument;
        [SerializeField] internal RadarMapController radarMapController;
        [SerializeField] internal SurfaceMapController surfaceMapController;

        void Start() {
            radarMapController.Initialize(this);
            surfaceMapController.Initialize(this);
        }
    }
}