using ATC.Operator.Networking;
using Pathway;
using UnityEngine;

namespace ATC.Operator.Airplane {
    public enum MovementType { none, arrival, departure }

    public class AirplaneController : MonoBehaviour {
        internal Vector3 position { get { return transform.position; } }
        internal Vector3 fwd { get { return transform.forward; } }

        [Header("Pre-Given Data")]
        [SerializeField] internal AirplaneData myData;
        [SerializeField] internal CallSign callSign;

        [Header("Dynamic Data & Type")]
        [SerializeField] internal ushort globalID;
        [SerializeField] internal MovementType movementType;
        [SerializeField] internal NameAndTimeZ lastNameAndTimeZ;
        [SerializeField] internal NameAndTimeZ nextNameAndTimeZ;
        [SerializeField] internal bool hasRadarMap;
        [SerializeField] internal bool hasSurfaceMap;
        [SerializeField] internal Path[] pathHistory;
        [SerializeField] internal ArrivalCommandID[] allArrivalCommandID;
        [SerializeField] internal DepartureCommandID[] allDepartureCommandID;

        [SerializeField] internal AP_Events apEvent;

        [SerializeField] internal AP_Movement apMovemenet = new AP_Movement();


        internal void Initialize(ushort rGlobalID, NetworkAirplane_SpawnInfo rSpawnInfo) {
            globalID = rGlobalID;
            callSign = rSpawnInfo.callSign;
            movementType = rSpawnInfo.movementType;
            lastNameAndTimeZ = rSpawnInfo.lastNT;
            nextNameAndTimeZ = rSpawnInfo.nextNT;
            hasRadarMap = rSpawnInfo.hasRadarMap; hasSurfaceMap = rSpawnInfo.hasSurfaceMap;
            pathHistory = rSpawnInfo.pathHistory;
            allArrivalCommandID = rSpawnInfo.allArrivalCommandID; allDepartureCommandID = rSpawnInfo.allDepartureCommandID;
            apEvent.Initialize(this);
            GlobalEvent.OnAirplaneCreated(globalID, this);
            if (hasRadarMap) {
                GlobalEvent.OnRadarMapNodeCreated(this, rSpawnInfo.startLineIndex_radarMap);
            }
            if (hasSurfaceMap) {
                GlobalEvent.OnSurfaceMapNodeCreated(this, rSpawnInfo.startLineIndex_surfaceMap);
            }
            apMovemenet.Initialize(this, rSpawnInfo.pos, rSpawnInfo.fwd);
            apMovemenet.Activate();
        }



        private void Update() {
            apMovemenet.ManualUpdate();
        }



        internal void Update_VizHeadSpeedFL(VizHeadSpeedFL rVizHeadSpeedFL) {
            apEvent.Update_VizHeadingSpeedFL(rVizHeadSpeedFL);
        }

        internal void Update_PathHistory(Path[] rPathHistory) {
            pathHistory = rPathHistory;
            apEvent.Update_PathHistory(rPathHistory);
        }

        internal void Update_AirplanePhaseNT(NameAndTimeZ rLastPhase, NameAndTimeZ rNextPhase) {
            apEvent.Update_AirplanePhaseNT(rLastPhase, rNextPhase);
        }
        internal void Update_ArrivalCommandList(ArrivalCommandID[] rAllArrivalCommandID) {
            allArrivalCommandID = rAllArrivalCommandID;
            apEvent.Update_ArrivalCommandList(rAllArrivalCommandID);
        }
        internal void Update_DepartureCommandList(DepartureCommandID[] rAllDepartureCommandID) {
            allDepartureCommandID = rAllDepartureCommandID;
            apEvent.Update_DepartureCommandList(rAllDepartureCommandID);
        }
        internal void OnDestroyRadapMapNode() {
            apEvent.DestroyRadarMapNode();
        }
        internal void OnDestroySurfaceMapNode() {
            apEvent.DestroySurfaceMapNode();
        }


        internal void DestroyMeAll() {
            apEvent.DestroyRadarMapNode();
            apEvent.DestroySurfaceMapNode();
            apEvent.DestroyThisAirplane();
            if (GlobalData.allActiveAirplane.ContainsKey(globalID)) {
                GlobalData.allActiveAirplane.Remove(globalID);
            }
            Destroy(gameObject);
        }
    }
}


