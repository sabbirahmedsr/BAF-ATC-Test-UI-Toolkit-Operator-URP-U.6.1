using UnityEngine;

namespace ATC.Operator.Airplane {
    public class AP_Movement {

        [Header("Interpolation")]
        private Vector3 curPos, tgtPos = new Vector3();
        private Quaternion curRot, tgtRot = Quaternion.identity;
        private float counter; private float speed;
        private float nextMapNodeUpdateTime;
        private float nextMapNodeUpdateTime_waitingTime = 0.1f;
        private Vector3 tmpPos, tmpFwd;

        private bool isActive;
        private Transform transform;

        AirplaneController apController;

        internal void Initialize(AirplaneController rApController, Vector3 rSpawnpos, Vector3 rSpawnFwd) {
            apController = rApController;
            transform = rApController.transform;

            transform.position = curPos = tgtPos = rSpawnpos;
            transform.rotation = curRot = tgtRot = Quaternion.LookRotation(rSpawnFwd);
        }

        internal void Activate() {
            isActive = true;
        }

        internal void ManualUpdate() {
            if (!isActive) {
                return;
            }
            if (counter <= 1f) {
                counter += Time.deltaTime * 5f;
                transform.position = Vector3.Lerp(curPos, tgtPos, counter);
                transform.rotation = Quaternion.Lerp(curRot, tgtRot, counter);
            }
            if (Time.time > nextMapNodeUpdateTime) {
                tmpPos = transform.position;
                tmpFwd = transform.forward;
                tmpPos.y = tmpFwd.y = 0; tmpFwd.Normalize();
                apController.apEvent.Update_MapNodePosRot(tmpPos, tmpFwd);
                nextMapNodeUpdateTime = Time.time + nextMapNodeUpdateTime_waitingTime;
            }
        }

        internal void Update_PosRot(Vector3 rPos, Vector3 rFwd) {
            curPos = transform.position; tgtPos = rPos;
            curRot = transform.rotation; tgtRot = Quaternion.LookRotation(rFwd);
            counter = 0f;
        }
    }
}
