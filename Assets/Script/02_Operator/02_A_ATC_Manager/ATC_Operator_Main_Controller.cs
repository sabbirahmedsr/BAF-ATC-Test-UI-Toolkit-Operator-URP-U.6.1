using UnityEngine;
using UnityEngine.UIElements;
using ATC.Operator.MapView;
using ATC.Operator.CommandView;
using ATC.Operator.FlightCreation;

namespace ATC.Operator {
    public class ATC_Operator_Main_Controller : MonoBehaviour {
        [SerializeField] internal UIDocument uiDocument;
        [SerializeField] internal Map_Controller radarMapController;
        [SerializeField] internal Map_Controller surfaceMapController;
        [SerializeField] internal CommandController commandController;
        [SerializeField] internal FlightCreation_Controller flightCreationController;

        [Header("Internat Helping Script")]
        [SerializeField] private ATC_Operator_Theme_Loader uiThemeLoader;

        void Start() {
            radarMapController.Initialize(this);
            surfaceMapController.Initialize(this);
            commandController.Initialize(this);
            uiThemeLoader.Initialize(this);
            flightCreationController.Initialize(this);
        }

    }
}