using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

namespace PuzzleForge
{
    [CustomEditor(typeof(PuzzleForgeSignalBase))]

    public class PuzzleForgeTriggerSignalEditor : Editor
    {
        override public void OnInspectorGUI()
        {
            PuzzleForgeSignalBase signalBase = (PuzzleForgeSignalBase)target;

            DrawDefaultInspector();

            if (GUILayout.Button("Test Enable"))
            {
                signalBase.DebugActivate();
            }

            if (GUILayout.Button("Test Disable"))
            {
                signalBase.DebugDeactivate();
            }
        }
    }
}