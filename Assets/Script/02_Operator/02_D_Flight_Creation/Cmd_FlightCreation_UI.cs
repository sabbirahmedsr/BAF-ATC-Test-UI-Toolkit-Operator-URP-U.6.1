using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace ATC.Operator.FlightCreation {
    public class Cmd_FlightCreation_UI {
        /*
        private VisualElement myRoot; // Corresponds to ui:VisualElement name="root"



        private VisualElement fligthCreationPopup; // Corresponds to ui:VisualElement name="fligthCreationRoot"
        private Label txtDescription; // Corresponds to ui:Label name="txtDescription"

        private DropdownField drdCategory; // Corresponds to ui:DropdownField name="drdCategory"
        private DropdownField drdCallSign; // Corresponds to ui:DropdownField name="drdCallSign"
        private DropdownField drdReportingPoint; // Corresponds to ui:DropdownField name="drdReportingPoint"
        private DropdownField drdParkingStand; // Corresponds to ui:DropdownField name="drdParkingStand"

        private Button btnConfirm; // Corresponds to ui:Button name="btnConfirm"
        private Button btnCancel; // Corresponds to ui:Button name="btnCancel"

      //  private Command_FlightCreation_Controller flightCreationCtrl;

        internal void Initialize(FlightCreation_Controller rFlightCreationCtrl) {
          //  flightCreationCtrl = rFlightCreationCtrl;

            VisualElement rootElement = rFlightCreationCtrl.cmdCtrl.mainController.uiDocument.rootVisualElement;
            myRoot = rootElement.Q<VisualElement>("cmdFlightCreationRoot");

            fligthCreationPopup = myRoot.Q<VisualElement>("fligthCreationPopup");
            txtDescription = myRoot.Q<Label>("txtDescription");
            
            drdCategory = myRoot.Q<DropdownField>("drdCategory");
            drdCallSign = myRoot.Q<DropdownField>("drdCallSign");
            drdReportingPoint = myRoot.Q<DropdownField>("drdReportingPoint");
            drdParkingStand = myRoot.Q<DropdownField>("drdParkingStand");

            btnConfirm = myRoot.Q<Button>("btnConfirm");
            btnCancel = myRoot.Q<Button>("btnCancel");

            // register callback
            drdCategory?.RegisterValueChangedCallback<string>(evt => { flightCreationCtrl.OnDrdChange_Catergory(drdCategory.index); });
            drdCallSign?.RegisterValueChangedCallback<string>(evt => { flightCreationCtrl.OnDrdChange_CallSign(drdCallSign.index); });
            drdReportingPoint?.RegisterValueChangedCallback<string>(evt => { flightCreationCtrl.OnDrdChange_ReportingPoint(drdReportingPoint.index); });
            drdParkingStand?.RegisterValueChangedCallback<string>(evt => { flightCreationCtrl.OnDrdChange_ParkingStand(drdParkingStand.index); });

            // Set up event listeners for buttons

            btnConfirm?.RegisterCallback<ClickEvent>(OnClick_Confirm);
            btnCancel?.RegisterCallback<ClickEvent>(OnClick_Cancel);
        }









        internal void Refresh_DrdCategoryOptions(int rValueIndex) {
            drdCategory.choices.Clear();
            for (int i = 0; i < flightCreationCtrl.result_category.Count; i++) { 
                drdCategory.choices.Add(flightCreationCtrl.result_category[i].ToString());
            }
            drdReportingPoint.index = rValueIndex;
        }
        internal void Refresh_DrdCallSignOptions(int rValueIndex) {
            drdCallSign.choices.Clear();
            for (int i = 0; i < flightCreationCtrl.result_apData.Count; i++) {
                drdCallSign.choices.Add(flightCreationCtrl.result_apData[i].callSign.ToString());
            }
            drdReportingPoint.index = rValueIndex;
        }
        internal void Refresh_DrdReportingPointOptions(int rValueIndex) {
            drdReportingPoint.choices.Clear();
            drdReportingPoint.choices = flightCreationCtrl.selectedApData.allPreArrivalPoints.ToList();
            drdReportingPoint.index = rValueIndex;
        }
        internal void Refresh_DrdParkingStandOptions(int rValueIndex) {
            drdParkingStand.choices.Clear();
            for (int i = 0; i < flightCreationCtrl.selectedApData.allParkingStandID.Length; i++) {
                drdParkingStand.choices.Add(flightCreationCtrl.selectedApData.allParkingStandID[i].ToString());
            }           
            drdParkingStand.index = rValueIndex;
        }






        private void OnClick_Confirm(ClickEvent evt) {
            flightCreationCtrl.Create_ArrFlight_OnServer();
            fligthCreationPopup.style.display = DisplayStyle.None;
        }

        private void OnClick_Cancel(ClickEvent evt) {
            fligthCreationPopup.style.display = DisplayStyle.None;
        }
        */
    }
}