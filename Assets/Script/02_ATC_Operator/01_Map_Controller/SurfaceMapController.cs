using UnityEngine;

namespace ATC.Operator.MapView {

    public enum SurfaceMapType { runway = 0, parking = 1 }

    public class SurfaceMapController : ATC_Operator_Sub_Controller {
        [SerializeField] private Map_View_Controller mapViewController;

        internal override void Initialize(ATC_Operator_Main_Controller _mainController) {
            base.Initialize(_mainController);
            mapViewController.Initialize(mainController.uiDocument);
        }


        /*
        [Header("Internal Class")]
        [SerializeField] private SurfaceMap_MapTypeUI mapTypeUI;


        [Header("Map Node")]
        [SerializeField] private GameObject surfaceMapNodePrefab;
        internal List<SurfaceMapNode> allMapNode = new List<SurfaceMapNode>();


        private void OnEnable() {
            GlobalEvent.onMapLoadedEvent += OnAirportMapLoaded;
            GlobalEvent.onSurfaceMapNodeCreatedEvent += OnSurfaceMapNodeCreated;
        }
        private void OnDisable() {
            GlobalEvent.onMapLoadedEvent -= OnAirportMapLoaded;
            GlobalEvent.onSurfaceMapNodeCreatedEvent -= OnSurfaceMapNodeCreated;
        }
        private void Start() {
            optionUI.Initialize(surfaceMapCamera);
            maximizeUI.Initialize(surfaceMapCamera);
            mapTypeUI.Initialize(surfaceMapCamera);
        }



        private void OnAirportMapLoaded(MapData rAirportMapData) {
            mapTypeUI.OnAirportMapLoaded(rAirportMapData);
        }
        private void OnSurfaceMapNodeCreated(AirplaneController rAPController, ushort rStartLineIndex) {
            GameObject newMapNodeObject = Instantiate(surfaceMapNodePrefab, tSurfaceContentRoot);
            SurfaceMapNode surfaceMapNode = newMapNodeObject.GetComponent<SurfaceMapNode>();
            surfaceMapNode.Initialize(this, rAPController, rStartLineIndex);
            allMapNode.Add(surfaceMapNode);
        }
        private void OnSwitchSurfaceMapType(SurfaceMapType rSurfaceMapType, MapTheme mapTheme) {
            foreach (SurfaceMapNode surfaceMapNode in allMapNode) {
                surfaceMapNode.Update_ChildScale();
            }
        }*/
    }
}
