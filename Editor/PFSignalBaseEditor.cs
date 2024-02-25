using UnityEditor;
using UnityEngine;

namespace PuzzleForge
{
    [CustomEditor(typeof(PFSignalEventBase), true)]
    public class PFSignalBaseEditor : Editor
    {
        public void OnSceneGUI()
        {
            var signalBase = (PFSignalEventBase)target;
            var controller = signalBase.parentController;
            if (controller == null)
                return;
            foreach (var hookup in controller.OnActivationActivateHookups)
            {
                if(hookup.signal == null) continue;
                foreach (var reactor in hookup.reactors)
                {
                    if(reactor == null) continue;
                    EditorHelper.DrawCurve(hookup.signal.transform.position, reactor.transform.position, Color.red,
                        0.7f);
                }
            }
            foreach (var hookup in controller.OnDeactivationDeactivateHookups)
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
            var signalBase = (PFSignalEventBase)target;

            signalBase.parentController?.RegisterSignal(signalBase);

            DrawDefaultInspector();
        }
    }
}