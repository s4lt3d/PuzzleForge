using UnityEngine;
using UnityEditor;

namespace PuzzleForge
{
    [CustomEditor(typeof(PuzzleForgeTriggerSource))]
    public class PuzzleForgeActivatorEditor : Editor
    {
        override public void OnInspectorGUI()
        {
            PuzzleForgeTriggerSource triggerSource = (PuzzleForgeTriggerSource)target;

            int selectedIndex = triggerSource.sendDisable ? 1 : 0;
            string[] options = new string[] { "Send Activate", "Send Deactivate" };
            selectedIndex = EditorGUILayout.Popup("On Start Interaction", selectedIndex, options);
            triggerSource.sendDisable = selectedIndex == 1;

            if (GUI.changed)
            {
                EditorUtility.SetDirty(target);
            }

            DrawDefaultInspector();

            if (GUILayout.Button("Test Enable"))
            {
                triggerSource.Enable();
            }

            if (GUILayout.Button("Test Disable"))
            {
                triggerSource.Disable();
            }
        }
    }
}