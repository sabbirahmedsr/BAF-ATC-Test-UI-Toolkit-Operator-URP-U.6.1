using UnityEngine;
using UnityEngine.UIElements;

namespace ATC.Operator.MapView {
    [System.Serializable]
    public class Map_Node_Info_UI {

        [Header("Text Variable")]
        private TextElement txtCallSign;
        private TextElement txtHeading;
        private TextElement txtSpeed;
        private TextElement txtHeight;

        [Header("Icon Variable")]
        private VisualElement icnUpArrow;
        private VisualElement icnDownArrow;
        private VisualElement icnFlatArrow;


        internal void Initialize(Map_Node map_Node, TemplateContainer mapNode_CloneElement) {
            
            // get text reference
            txtCallSign = mapNode_CloneElement.Q<TextElement>("txtCallSign");
            txtHeading = mapNode_CloneElement.Q<TextElement>("txtHeading");
            txtSpeed = mapNode_CloneElement.Q<TextElement>("txtSpeed");
            txtHeight = mapNode_CloneElement.Q<TextElement>("txtHeight");

            // get icon reference
            icnUpArrow = mapNode_CloneElement.Q<VisualElement>("icnUpArrow");
            icnDownArrow = mapNode_CloneElement.Q<VisualElement>("icnDownArrow");
            icnFlatArrow = mapNode_CloneElement.Q<VisualElement>("icnFlatArrow");

            // set initial variable
            txtCallSign.text = map_Node.apController.callSign.ToString();
        }

        internal void SetVizHeadSpeedHeight(VizHeadSpeedFL rVizHeadSpeedFL) {
            // set text
            txtHeading.text = rVizHeadSpeedFL.heading.ToString();
            txtSpeed.text = rVizHeadSpeedFL.speed.ToString();
            txtHeight.text = rVizHeadSpeedFL.FL.ToString();

            // set icon
            icnUpArrow.style.display = rVizHeadSpeedFL.flDirection == FLDirection.upward? DisplayStyle.Flex : DisplayStyle.None;
            icnDownArrow.style.display = rVizHeadSpeedFL.flDirection == FLDirection.downward ? DisplayStyle.Flex : DisplayStyle.None;
            icnFlatArrow.style.display = rVizHeadSpeedFL.flDirection == FLDirection.flat ? DisplayStyle.Flex : DisplayStyle.None;
        }
    }
}