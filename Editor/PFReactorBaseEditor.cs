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
            var reactorBase = (PFReactorBase)target;

            reactorBase?.parentController?.RegisterReactor(reactorBase);

            DrawDefaultInspector();
        }
    }
}