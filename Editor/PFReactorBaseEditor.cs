using UnityEditor;
using UnityEngine;

namespace PuzzleForge
{
    [CustomEditor(typeof(PFReactorBase), true)]
    public class PFReactorBaseEditor : Editor
    {
        public void OnSceneGUI()
        {
            var signalBase = (PFReactorBase)target;
            var controller = signalBase.parentController;
            if (controller == null)
                return;
            foreach (var hookup in controller.activationHookups)
            {
                foreach (var reactor in hookup.reactors)
                    EditorHelper.DrawCurve(hookup.signal.transform.position, reactor.transform.position, Color.red,
                        0.7f);
            }

            foreach (var hookup in controller.deactivationHookups)
            {
                foreach (var reactor in hookup.reactors)
                    EditorHelper.DrawCurve(hookup.signal.transform.position, reactor.transform.position, Color.red,
                        0.7f);
            }
        }

        public override void OnInspectorGUI()
        {
            var reactorBase = (PFReactorBase)target;

            reactorBase?.parentController?.RegisterReactor(reactorBase);

            DrawDefaultInspector();
        }
    }
}