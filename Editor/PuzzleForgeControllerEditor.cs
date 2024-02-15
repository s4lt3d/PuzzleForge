using UnityEngine;
using UnityEditor;

namespace PuzzleForge
{
    [CustomEditor(typeof(PuzzleForgeController))]
    public class PuzzleForgeControllerEditor : Editor
    {
        private PuzzleForgeController selectedObject;
        bool[,] enableFieldsArray = new bool[4, 4];
        bool[,] disableFieldsArray = new bool[4, 4];
        PuzzleForgeReactor[] reactors;
        PuzzleForgeActivator[] activators;
        GameObject selected = null;

        PuzzleForgeController switchController;

        private void DrawCurve(Vector3 pos1, Vector3 pos2, Color color, float curveRatio)
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
                2
            );
        }

        public void OnEnable()
        {
            if (target != null)
                selectedObject = target as PuzzleForgeController;
            reactors = selectedObject.GetReactors();
            activators = selectedObject.GetActivators();
            GetActivationMasks();
        }

        public void OnSceneGUI()
        {
            //    Event.current.Use();

            if (selected != null)
                Handles.Label(selected.transform.position + Vector3.up * 2, selected.name);


            switchController = target as PuzzleForgeController;
            if (switchController == null) return;

            // Example drawing a wire cube around the SwitchController object
            Handles.DrawWireCube(switchController.transform.position, Vector3.one * 1.5f);

            // Drawing curves to reactors and activators, assuming these methods are correctly implemented
            foreach (PuzzleForgeReactor reactor in switchController.objectsToActivate)
            {
                DrawCurve(switchController.transform.position, reactor.transform.position, Color.blue, 0.85f);
            }

            foreach (PuzzleForgeActivator activator in switchController.objectsWhichActivate)
            {
                DrawCurve(switchController.transform.position, activator.transform.position, Color.red, 0.7f);
            }
        }



        public override void OnInspectorGUI()
        {

            DrawDefaultInspector();
            switchController = (PuzzleForgeController)target;

            if (selectedObject == null)
                return;

            reactors = selectedObject.GetReactors();
            activators = selectedObject.GetActivators();

            DrawToggleMatrix();
        }

        void DrawToggleMatrix()
        {
            int longestName = 150;
            int longestReactorName = 150;

            Rect r = GUILayoutUtility.GetRect(longestReactorName + activators.Length * 15, longestName + reactors.Length * 30 + 30);

            GUIUtility.RotateAroundPivot(-90, new Vector2(r.x + longestReactorName, r.y));
            for (int i = 0; i < activators.Length; i++)
            {
                //         EditorGUI.LabelField(new Rect(r.x, r.y + i * 15, longestName, 15), reactors[i].name);
                if (GUI.Button(new Rect(r.x + longestReactorName - longestName, r.y + i * 15, longestName, 15), activators[i].name, GUI.skin.label))
                {
                    selected = activators[i].gameObject;
                }
            }
            GUIUtility.RotateAroundPivot(90, new Vector2(r.x + longestReactorName, r.y));

            for (int j = 0; j < reactors.Length; j++)
            {
                EditorGUI.LabelField(new Rect(r.x, r.y + j * 30 + longestName, longestReactorName, 15), "Enable " + reactors[j].name);
                EditorGUI.LabelField(new Rect(r.x, r.y + j * 30 + longestName + 15, longestReactorName, 15), "Disable " + reactors[j].name);

                for (int i = 0; i < activators.Length; i++)
                {
                    enableFieldsArray[j, i] = EditorGUI.Toggle(new Rect(r.x + i * 15 + longestReactorName, r.y + j * 30 + longestName, 15, 15), enableFieldsArray[j, i]);
                }

                for (int i = 0; i < activators.Length; i++)
                {
                    disableFieldsArray[j, i] = EditorGUI.Toggle(new Rect(r.x + i * 15 + longestReactorName, r.y + j * 30 + longestName + 15, 15, 15), disableFieldsArray[j, i]);
                }
            }

            if (GUI.changed)
            {
                SetActivationMasks();
            }
        }

        private void SetActivationMasks()
        {
            for (int i = 0; i < reactors.Length; i++)
            {
                ulong activationMask = 0;
                ulong deactivationMask = 0;

                for (int j = 0; j < activators.Length; j++)
                {
                    if (enableFieldsArray[i, j] == true)
                    {
                        activationMask |= (ulong)1 << j;
                    }
                    if (disableFieldsArray[i, j] == true)
                    {
                        deactivationMask |= (ulong)1 << j;
                    }
                }
                reactors[i].enableMask = activationMask;
                reactors[i].disableMask = deactivationMask;
                EditorUtility.SetDirty(reactors[i]);
            }

            for (int j = 0; j < activators.Length; j++)
            {
                activators[j].actionLayer = (ulong)1 << j;
                EditorUtility.SetDirty(activators[j]);
            }
        }

        private void GetActivationMasks()
        {
            enableFieldsArray = new bool[reactors.Length, activators.Length];
            disableFieldsArray = new bool[reactors.Length, activators.Length];

            for (int i = 0; i < reactors.Length; i++)
            {
                for (int j = 0; j < activators.Length; j++)
                {
                    if ((reactors[i].enableMask & ((ulong)1 << j)) > 0)
                    {
                        enableFieldsArray[i, j] = true;
                    }
                    else
                        enableFieldsArray[i, j] = false;

                    if ((reactors[i].disableMask & ((ulong)1 << j)) > 0)
                    {
                        disableFieldsArray[i, j] = true;
                    }
                    else
                        disableFieldsArray[i, j] = false;
                }
            }
        }
    }
}