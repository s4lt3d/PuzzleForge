using UnityEditor;

namespace PuzzleForge
{
    [CustomEditor(typeof(PuzzleForgeBase), true)]
    public class PuzzleForgeBaseEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            PuzzleForgeBase baseclass = (PuzzleForgeBase)target;
            
            DrawDefaultInspector();
        }
    }
}