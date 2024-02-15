using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace PuzzleForge
{
    [CustomEditor(typeof(PuzzleForgeActivator))]
    public class ActivatorEditor : Editor
    {
        override public void OnInspectorGUI()
        {
            PuzzleForgeActivator activator = (PuzzleForgeActivator)target;

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