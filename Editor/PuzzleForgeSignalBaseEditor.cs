using UnityEditor;

namespace PuzzleForge
{
    [CustomEditor(typeof(PuzzleForgeSignalBase), true)]
    public class PuzzleForgeSignalBaseEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            PuzzleForgeSignalBase signalBase = (PuzzleForgeSignalBase)target;

            signalBase.parentController?.RegisterSignal(signalBase);
            
            DrawDefaultInspector();
        }
    }
}