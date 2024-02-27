﻿using UnityEditor;
using UnityEngine;

namespace PuzzleForge
{
    [CustomEditor(typeof(PFSignalEventBase), true)]
    public class PFSignalBaseEditor : Editor
    {
        public void OnSceneGUI()
        {
            var signal = (PFSignalEventBase)target;

            var reactors = signal.GetAllReactors();

            foreach (var reactor in reactors)
            {
                EditorHelper.DrawCurve(signal.transform.position, reactor.transform.position, Color.red,
                    0.7f);
            }
        }
    }
}