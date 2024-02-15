using UnityEngine;
using UnityEditor;

namespace PuzzleForge
{
    [CustomEditor(typeof(PuzzleForgeActivator))]
    public class PuzzleForgeActivatorEditor : Editor
    {
        override public void OnInspectorGUI()
        {
            PuzzleForgeActivator activator = (PuzzleForgeActivator)target;

            int selectedIndex = activator.sendDisable ? 1 : 0;
            string[] options = new string[] { "Send Activate", "Send Deactivate" };
            selectedIndex = EditorGUILayout.Popup("On Interaction", selectedIndex, options);
            activator.sendDisable = selectedIndex == 1;

            if (GUI.changed)
            {
                EditorUtility.SetDirty(target);
            }

            DrawDefaultInspector();

            if (GUILayout.Button("Test Enable"))
            {
                activator.Enable();
            }

            if (GUILayout.Button("Test Disable"))
            {
                activator.Disable();
            }
        }
    }
}