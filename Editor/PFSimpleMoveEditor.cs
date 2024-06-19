using UnityEditor;
using UnityEngine;

namespace PuzzleForge
{
#if UNITY_EDITOR

    [CustomEditor(typeof(PFSimpleMover))]
    public class PFSimpleMoverEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            // Draw the default inspector
            DrawDefaultInspector();

            // Get reference to the PFSimpleMover script
            var myScript = (PFSimpleMover)target;

            // If we press this button, call CapturePosition on script
            if (GUILayout.Button("Capture Position"))
            {
                myScript.CapturePosition();
            }

            // If we press this button, it will instantly cycle to the next position
            if (GUILayout.Button("Cycle to Next Position"))
            {
                myScript.CyclePosition(true);
            }

            // If we press this button, call Reset on script
            if (GUILayout.Button("Reset"))
            {
                myScript.Reset(true);
                
            }
        }
    }
#endif
}