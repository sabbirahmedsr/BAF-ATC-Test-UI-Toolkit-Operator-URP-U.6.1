using ATC.Global;
using System;
using UnityEngine.UIElements;

namespace ATC.Operator.FlightCreation {
    /// <summery>
    /// Arrival Command Controller Create Arrival Aircraft/Flight
    /// </summery>
    public class FlightCreation_Controller : ATC_Operator_Sub_Controller {

        private Button btnCreateArrivalAirplane;
        private Button btnCreateDepartureAirplane;

        private Flight_Creation_Window fc_window = new Flight_Creation_Window();

        internal override void Initialize(ATC_Operator_Main_Controller _mainController) {
            base.Initialize(_mainController);
            //
            FindUIReference(_mainController.uiDocument.rootVisualElement);
            RegisterCallback();
            fc_window.Initialize(this);
        }

        private void FindUIReference(VisualElement rootElement) {
            VisualElement myRoot = rootElement.Q<VisualElement>("flightCreationRoot");
            // action button
            btnCreateArrivalAirplane = myRoot.Q<Button>("btnCreateArrivalAirplane");
            btnCreateDepartureAirplane = myRoot.Q<Button>("btnCreateDepartureAirplane");
        }
        private void RegisterCallback() {
            btnCreateArrivalAirplane?.RegisterCallback<ClickEvent>(OnClick_CreateArrivalAirplane);
            btnCreateDepartureAirplane?.RegisterCallback<ClickEvent>(OnClick_CreateDepartureAirplane);
        }
        private void UnregisterCallback() {
            btnCreateArrivalAirplane?.UnregisterCallback<ClickEvent>(OnClick_CreateArrivalAirplane);
            btnCreateDepartureAirplane?.UnregisterCallback<ClickEvent>(OnClick_CreateDepartureAirplane);
        }


        private void OnClick_CreateArrivalAirplane(ClickEvent evt) {
            fc_window.Activate(true);
        }

        private void OnClick_CreateDepartureAirplane(ClickEvent evt) {
            fc_window.Activate(true);
        }

    }
}