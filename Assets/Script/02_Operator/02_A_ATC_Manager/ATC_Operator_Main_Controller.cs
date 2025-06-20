using UnityEngine;
using UnityEngine.UIElements;
using ATC.Operator.MapView;

namespace ATC.Operator {
    public class ATC_Operator_Main_Controller : MonoBehaviour {
        [SerializeField] internal UIDocument uiDocument;
        [SerializeField] internal Map_Controller radarMapController;
        [SerializeField] internal Map_Controller surfaceMapController;

        [Header("Internat Helping Script")]
        [SerializeField] private ATC_Operator_Theme_Loader uiThemeLoader;
        void Start() {
            radarMapController.Initialize(this);
            surfaceMapController.Initialize(this);
            uiThemeLoader.Initialize(this);
        }
    }
}