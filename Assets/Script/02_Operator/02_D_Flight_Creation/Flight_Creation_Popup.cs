using ATC.Global;
using ATC.Operator;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace ATC.Operator.FlightCreation {
    public class Flight_Creation_Window {

        internal TypeOfAircraft selectedCategory = TypeOfAircraft.International;
        internal AirplaneData selectedApData = null;
        internal ushort selectedRerportingPoint = 0;
        internal ParkingStandID selectedParkingStandID = ParkingStandID._05;

        internal List<TypeOfAircraft> result_category = new List<TypeOfAircraft>();
        internal List<AirplaneData> result_apData = new List<AirplaneData>();


        [Header("Ui Reference")]
        private VisualElement myRoot; // Corresponds to ui:VisualElement name="root"
        private Label txtDescription; // Corresponds to ui:Label name="txtDescription"
        // dropdown
        private DropdownField drdCategory; // Corresponds to ui:DropdownField name="drdCategory"
        private DropdownField drdCallSign; // Corresponds to ui:DropdownField name="drdCallSign"
        private DropdownField drdReportingPoint; // Corresponds to ui:DropdownField name="drdReportingPoint"
        private DropdownField drdParkingStand; // Corresponds to ui:DropdownField name="drdParkingStand"
        // action button
        private Button btnConfirm; // Corresponds to ui:Button name="btnConfirm"
        private Button btnCancel; // Corresponds to ui:Button name="btnCancel"

        [Header("Parent Script")]
        private FlightCreation_Controller flightCreationCtrl;


        internal void Initialize(FlightCreation_Controller rFlightCreationCtrl) {
            flightCreationCtrl = rFlightCreationCtrl;
            FindUIReference(rFlightCreationCtrl.mainController.uiDocument.rootVisualElement);
            RegisterCallback();
        }
        internal void Activate(bool rBool) {
            myRoot.style.display = rBool ? DisplayStyle.Flex : DisplayStyle.None;
        }





        private void FindUIReference(VisualElement rootElement) {
            myRoot = rootElement.Q<VisualElement>("flightCreationPopup");
            txtDescription = myRoot.Q<Label>("txtDescription");
            // dropdown
            drdCategory = myRoot.Q<DropdownField>("drdCategory");
            drdCallSign = myRoot.Q<DropdownField>("drdCallSign");
            drdReportingPoint = myRoot.Q<DropdownField>("drdReportingPoint");
            drdParkingStand = myRoot.Q<DropdownField>("drdParkingStand");
            // action button
            btnConfirm = myRoot.Q<Button>("btnConfirm");
            btnCancel = myRoot.Q<Button>("btnCancel");
        }
        private void RegisterCallback() {
            drdCategory?.RegisterValueChangedCallback<string>(evt => { OnDrdChange_Catergory(drdCategory.index); });
            drdCallSign?.RegisterValueChangedCallback<string>(evt => { OnDrdChange_CallSign(drdCallSign.index); });
            drdReportingPoint?.RegisterValueChangedCallback<string>(evt => { OnDrdChange_ReportingPoint(drdReportingPoint.index); });
            drdParkingStand?.RegisterValueChangedCallback<string>(evt => { OnDrdChange_ParkingStand(drdParkingStand.index); });
            // Set up event listeners for buttons
            btnConfirm?.RegisterCallback<ClickEvent>(OnClick_Confirm);
            btnCancel?.RegisterCallback<ClickEvent>(OnClick_Cancel);
        }
        private void UnregisterCallback() {
            drdCategory?.UnregisterValueChangedCallback<string>(evt => { OnDrdChange_Catergory(drdCategory.index); });
            drdCallSign?.UnregisterValueChangedCallback<string>(evt => { OnDrdChange_CallSign(drdCallSign.index); });
            drdReportingPoint?.UnregisterValueChangedCallback<string>(evt => { OnDrdChange_ReportingPoint(drdReportingPoint.index); });
            drdParkingStand?.UnregisterValueChangedCallback<string>(evt => { OnDrdChange_ParkingStand(drdParkingStand.index); });
            // Set up event listeners for buttons
            btnConfirm?.UnregisterCallback<ClickEvent>(OnClick_Confirm);
            btnCancel?.UnregisterCallback<ClickEvent>(OnClick_Cancel);
        }






        public void OnDrdChange_Catergory(int rIndex) {
            selectedCategory = result_category[rIndex];
            Refresh_DrdCallSignOptions();
            Refresh_DrdReportingPointOptions();
        }
        public void OnDrdChange_CallSign(int rIndex) {
            selectedApData = result_apData[rIndex];
            Refresh_DrdReportingPointOptions();
        }
        public void OnDrdChange_ReportingPoint(int rIndex) {
            selectedRerportingPoint = (ushort)rIndex;
        }
        public void OnDrdChange_ParkingStand(int rIndex) {
            selectedParkingStandID = selectedApData.allParkingStandID[rIndex];
        }








        internal void Refresh_DrdCategoryOptions() {
            // set the restul first
            TypeOfAircraft[] allCategory = (TypeOfAircraft[])Enum.GetValues(typeof(TypeOfAircraft));
            result_category.Clear();
            // Note : we have to skip the first type as it is set as none [thats why, i = 1]
            for (int i = 1; i < allCategory.Length; i++) {
                result_category.Add(allCategory[i]);
            }
            selectedCategory = allCategory[0];

            // now set the dropdown
            drdCategory.choices.Clear();
            for (int i = 0; i < result_category.Count; i++) {
                drdCategory.choices.Add(result_category[i].ToString());
            }
            drdReportingPoint.index = 0;
        }
        internal void Refresh_DrdCallSignOptions() {
            result_apData.Clear();
            /*  for (int i = 0; i < cmdCtrl.allAirplaneData.Length; i++) {
                  if (cmdCtrl.allAirplaneData[i].aircraftType == selectedCategory) {
                      result_apData.Add(cmdCtrl.allAirplaneData[i]);
                  }
              }*/
            UnityEngine.Debug.Log(result_apData.Count);
            selectedApData = result_apData[0];
            //
            drdCallSign.choices.Clear();
            for (int i = 0; i < result_apData.Count; i++) {
                drdCallSign.choices.Add(result_apData[i].callSign.ToString());
            }
            drdReportingPoint.index = drdCallSign.choices.Count-1;
        }
        internal void Refresh_DrdReportingPointOptions() {
            int lastIndex = selectedApData.allPreArrivalPoints.Length - 1;
            selectedRerportingPoint = (ushort)lastIndex;
            //
            drdReportingPoint.choices.Clear();
            drdReportingPoint.choices = selectedApData.allPreArrivalPoints.ToList();
            drdReportingPoint.index = lastIndex;
        }
        internal void Refresh_DrdParkingStandOptions(int rValueIndex) {
            drdParkingStand.choices.Clear();
            for (int i = 0; i < selectedApData.allParkingStandID.Length; i++) {
                drdParkingStand.choices.Add(selectedApData.allParkingStandID[i].ToString());
            }
            drdParkingStand.index = rValueIndex;
        }





        private void OnClick_Confirm(ClickEvent evt) {
            Create_ArrFlight_OnServer();
            myRoot.style.display = DisplayStyle.None;
        }

        private void OnClick_Cancel(ClickEvent evt) {
            myRoot.style.display = DisplayStyle.None;
        }








        internal void Create_ArrFlight_OnServer() {
            GlobalNetwork.actionSender.Create_ArrFlight_OnServer(selectedApData.callSign, selectedRerportingPoint);
        }

        internal void Create_DepFlight_OnServer() {
            GlobalNetwork.actionSender.Create_DepFlight_OnServer(selectedApData.callSign, selectedParkingStandID);
        }
    }
}
