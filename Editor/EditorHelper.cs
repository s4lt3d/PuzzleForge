

using UnityEditor;
using UnityEngine;

namespace PuzzleForge
{
    public static class EditorHelper
    {
        
        public static void DrawCurve(Vector3 pos1, Vector3 pos2, Color color, float curveRatio, int width=1)
        {
            Vector3 start = pos1;

            Vector3 end = pos2;

            float offset = Mathf.Abs(start.y - end.y) * curveRatio;

            Handles.DrawBezier(
                start,
                end,
                start - Vector3.up * offset,
                end + Vector3.up * offset,
                color,
                EditorGUIUtility.whiteTexture,
                width
            );
        }
    }
}