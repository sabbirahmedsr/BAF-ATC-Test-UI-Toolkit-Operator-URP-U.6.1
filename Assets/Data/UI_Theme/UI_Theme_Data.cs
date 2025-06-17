using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace ATC.Data {
    [CreateAssetMenu(fileName = "ATC_UI_Theme_Data", menuName = "Azmi_Studio/ATC_UI_Theme")]
    public class UI_Theme_Data : ScriptableObject {
        [SerializeField] internal UI_Theme selectedUITheme;
        [SerializeField] internal UI_Theme[] allUITheme = new UI_Theme[1];

        internal List<string> GetAllThemeName() {
            List<string> result = new List<string>();
            foreach (UI_Theme theme in allUITheme) {
                result.Add(theme.caption.ToUpper());
            }
            return result;
        }

        internal UI_Theme GetUITheme(int index) {
            if (index < 0 || index >= allUITheme.Length) {
                Debug.LogError("No ui theme available with given index " + index);
                return null;
            }
            return allUITheme[index];
        }
    }

    [System.Serializable]
    internal class UI_Theme {
        [SerializeField] internal string caption;
        [SerializeField] internal Color mainColor;
        [SerializeField] internal Color backgroundColor;
        [SerializeField] internal StyleSheet styleSheet;
    }
}