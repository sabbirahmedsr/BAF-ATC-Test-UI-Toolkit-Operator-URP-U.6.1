using UnityEngine;
using ATC.Home.UI;

namespace ATC.Home {
    public class ATC_Home_Controller : MonoBehaviour {
        [SerializeField] private ATC_Home_UIManager uiManager;

        private void Start() {
            uiManager.ManualStart();
        }
    }
}