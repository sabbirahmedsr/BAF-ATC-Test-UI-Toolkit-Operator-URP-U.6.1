using ATC.Global;
using UnityEngine;
using UnityEngine.UIElements;

namespace ATC.Operator.CommandView {
    public class Command_Node_FixedCommand {

        // References to your UI Elements
        private Button btnHighlight;
        private Button btnFreeze;
        private Button btnResume;

        Command_Node cmdNode;

        internal void Initialize(Command_Node rCmdNode) {
            var nodeRoot = rCmdNode.cmdNode_cloneElement;

            // get ui reference
            btnHighlight = nodeRoot.Q<Button>("btnHighlight");
            btnFreeze = nodeRoot.Q<Button>("btnFreeze");
            btnResume = nodeRoot.Q<Button>("btnResume");


            // Register callback
            btnHighlight?.RegisterCallback<ClickEvent>(OnClick_Highlight);
            btnFreeze?.RegisterCallback<ClickEvent>(OnClick_Freeze);
            btnResume?.RegisterCallback<ClickEvent>(OnClick_Resume);
        }

        internal void UnregisterCallback() {
            btnHighlight?.UnregisterCallback<ClickEvent>(OnClick_Highlight);
            btnFreeze?.UnregisterCallback<ClickEvent>(OnClick_Freeze);
            btnResume?.UnregisterCallback<ClickEvent>(OnClick_Resume);
        }


        private void OnClick_Highlight(ClickEvent evt) {
            Debug.Log("Highlight button clicked!");
        }

        private void OnClick_Freeze(ClickEvent evt) {
            Debug.Log("Freeze button clicked!");
        }

        private void OnClick_Resume(ClickEvent evt) {
            Debug.Log("Resume button clicked!");
        }

    }
}