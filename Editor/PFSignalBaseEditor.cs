﻿using UnityEditor;
using UnityEngine;

namespace PuzzleForge
{
    [CustomEditor(typeof(PFSignalBase), true)]
    public class PFSignalBaseEditor : Editor
    {
        public void OnSceneGUI()
        {
            var signalBase = (PFSignalBase)target;
            var controller = signalBase.parentController;
            if (controller == null)
                return;
            foreach (var hookup in controller.activationHookups)
            {
                if(hookup.signal == null) continue;
                foreach (var reactor in hookup.reactors)
                {
                    if(reactor == null) continue;
                    EditorHelper.DrawCurve(hookup.signal.transform.position, reactor.transform.position, Color.red,
                        0.7f);
                }
            }
            foreach (var hookup in controller.deactivationHookups)
            {
                if(hookup.signal == null) continue;
                foreach (var reactor in hookup.reactors)
                {
                    if(reactor == null) continue;
                    EditorHelper.DrawCurve(hookup.signal.transform.position, reactor.transform.position, Color.red,
                        0.7f);
                }
            }
        }

        public override void OnInspectorGUI()
        {
            var signalBase = (PFSignalBase)target;

            signalBase.parentController?.RegisterSignal(signalBase);

            DrawDefaultInspector();
        }
    }
}