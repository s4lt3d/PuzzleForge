using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

namespace PuzzleForge
{
    [CustomEditor(typeof(PuzzleForgeTriggerSignal))]

    public class PuzzleForgeTriggerSignalEditor : Editor
    {
        override public void OnInspectorGUI()
        {
            PuzzleForgeTriggerSignal triggerSignal = (PuzzleForgeTriggerSignal)target;

            DrawDefaultInspector();

            if (GUILayout.Button("Test Enable"))
            {
                triggerSignal.DebugActivate();
            }

            if (GUILayout.Button("Test Disable"))
            {
                triggerSignal.DebugDeactivate();
            }
        }
    }
}