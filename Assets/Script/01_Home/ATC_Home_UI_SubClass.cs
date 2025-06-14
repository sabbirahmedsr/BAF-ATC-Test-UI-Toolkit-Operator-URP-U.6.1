using UnityEngine.UIElements;

namespace ATC.Home.UI {
    public class ATC_Home_UI_SubClass {
        internal VisualElement myRoot;
        internal ATC_Home_UIManager uiManager;

        internal virtual void Initialize(ATC_Home_UIManager _uiManager, VisualElement _rootElement, string _myRootName) {
            uiManager = _uiManager;
            myRoot = _rootElement.Q<VisualElement>(_myRootName);
        }

        internal virtual void Activate(bool rBool) {
            myRoot.style.display = rBool ? DisplayStyle.Flex : DisplayStyle.None;
        }

    }
}