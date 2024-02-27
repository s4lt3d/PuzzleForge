using UnityEditor;
using UnityEngine;

namespace PuzzleForge
{
    public static class EditorUtils
    {
        public static void DrawCurve(Vector3 pos1, Vector3 pos2, Color color, float curveRatio, int width = 1)
        {
            var start = pos1;
            var end = pos2;

            if (start.y < end.y) (start, end) = (end, start);

            var offset = Mathf.Abs(start.y - end.y) * curveRatio;

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