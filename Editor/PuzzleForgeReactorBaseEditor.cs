using UnityEditor;

namespace PuzzleForge
{
    [CustomEditor(typeof(PuzzleForgeReactorBase), true)]
    public class PuzzleForgeReactorBaseEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            PuzzleForgeReactorBase reactorBase = (PuzzleForgeReactorBase)target;

            reactorBase?.parentController?.RegisterReactor(reactorBase);
            
            DrawDefaultInspector();
        }
    }
}