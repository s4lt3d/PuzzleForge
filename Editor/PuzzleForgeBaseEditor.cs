﻿using UnityEditor;
using UnityEngine;

namespace PuzzleForge
{
    [CustomEditor(typeof(PuzzleForgeBase), true)]
    public class PuzzleForgeBaseEditor : Editor
    {
        public void OnSceneGUI()
        {
            var signalBase = (PuzzleForgeSignalBase)target;
            var controller = signalBase.parentController;
            if (controller == null)
                return;
            foreach (var hookup in controller.activationHookups)
                foreach (var reactor in hookup.reactors)
                    EditorHelper.DrawCurve(hookup.signal.transform.position, reactor.transform.position, Color.red,
                        0.7f);

            foreach (var hookup in controller.deactivationHookups)
                foreach (var reactor in hookup.reactors)
                    EditorHelper.DrawCurve(hookup.signal.transform.position, reactor.transform.position, Color.red,
                        0.7f);
        }

        public override void OnInspectorGUI()
        {
            var baseclass = (PuzzleForgeBase)target;

            DrawDefaultInspector();
        }
    }
}