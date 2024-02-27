using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace PuzzleForge
{
    [CustomEditor(typeof(PFReactorBase), true)]
    public class PFReactorBaseEditor : Editor
    {
        private HashSet<PFSignalEventBase> activators = new HashSet<PFSignalEventBase>();
        
        public void OnSceneGUI()
        {
            PFSignalEventBase[] allSignalEvents = FindObjectsOfType<PFSignalEventBase>();
            
            var reactor = (PFReactorBase)target;
            if(reactor == null) return;

            foreach (var signal in allSignalEvents)
            {
                if(signal == null) continue;
                
                if (signal.Contains(reactor))
                {
                    activators.Add(signal);
                }
            }

            foreach (var signal in activators)
            {
                EditorUtils.DrawCurve(signal.transform.position, reactor.transform.position, Color.red,
                    0.7f);
            }
        }
    }
}