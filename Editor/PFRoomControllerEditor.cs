using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace PuzzleForge
{
    [CustomEditor(typeof(PFRoomController))]
    public class PFRoomControllerEditor : Editor
    {
        private readonly int longestReactorName = 150;

        private readonly int                         toggleSize = 15;
        private          List<PFSignalBase> activators;
        private          bool[,]                     disableFieldsArray = new bool[4, 4];

        private bool[,] enableFieldsArray = new bool[4, 4];

        private List<PFReactorBase> reactors;

        private GameObject                selected = null;
        private PFRoomController switchRoomController;

        public void OnSceneGUI()
        {
            if (switchRoomController == null)
                return;
            foreach (var hookup in switchRoomController.activationHookups)
                foreach (var reactor in hookup.reactors)
                    EditorHelper.DrawCurve(hookup.signal.transform.position, reactor.transform.position, Color.red,
                        0.7f);

            foreach (var hookup in switchRoomController.deactivationHookups)
                foreach (var reactor in hookup.reactors)
                    EditorHelper.DrawCurve(hookup.signal.transform.position, reactor.transform.position, Color.red,
                        0.7f);
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            switchRoomController = (PFRoomController)target;
            activators           = switchRoomController.signals;
            reactors             = switchRoomController.reactors;

            enableFieldsArray  = new bool[reactors.Count, activators.Count];
            disableFieldsArray = new bool[reactors.Count, activators.Count];


            InitializeToggleStates();

            EditorGUI.BeginChangeCheck();

            DrawToggleMatrix();

            if (EditorGUI.EndChangeCheck())
            {
                switchRoomController.activationHookups.Clear();
                switchRoomController.deactivationHookups.Clear();

                for (var activatorIndex = 0; activatorIndex < activators.Count; activatorIndex++)
                {
                    var activationEntry = new PFActivationHookupEntry
                        { signal = activators[activatorIndex], reactors = new List<PFReactorBase>() };
                    var deactivationEntry = new PFActivationHookupEntry
                        { signal = activators[activatorIndex], reactors = new List<PFReactorBase>() };

                    for (var reactorIndex = 0; reactorIndex < reactors.Count; reactorIndex++)
                    {
                        if (enableFieldsArray[reactorIndex, activatorIndex])
                            activationEntry.reactors.Add(reactors[reactorIndex]);

                        if (disableFieldsArray[reactorIndex, activatorIndex])
                            deactivationEntry.reactors.Add(reactors[reactorIndex]);
                    }

                    if (activationEntry.reactors.Count > 0) switchRoomController.activationHookups.Add(activationEntry);

                    if (deactivationEntry.reactors.Count > 0)
                        switchRoomController.deactivationHookups.Add(deactivationEntry);
                }

                EditorUtility.SetDirty(target);
            }
        }

        private void InitializeToggleStates()
        {
            for (var j = 0; j < reactors.Count; j++)
                for (var i = 0; i < activators.Count; i++)
                {
                    enableFieldsArray[j, i]  = false;
                    disableFieldsArray[j, i] = false;
                }

            foreach (var entry in switchRoomController.activationHookups)
            {
                var activatorIndex = activators.IndexOf(entry.signal);
                if (activatorIndex != -1)
                    foreach (var reactor in entry.reactors)
                    {
                        var reactorIndex = reactors.IndexOf(reactor);
                        if (reactorIndex != -1) enableFieldsArray[reactorIndex, activatorIndex] = true;
                    }
            }

            foreach (var entry in switchRoomController.deactivationHookups)
            {
                var activatorIndex = activators.IndexOf(entry.signal);
                if (activatorIndex != -1)
                    foreach (var reactor in entry.reactors)
                    {
                        var reactorIndex = reactors.IndexOf(reactor);
                        if (reactorIndex != -1) disableFieldsArray[reactorIndex, activatorIndex] = true;
                    }
            }
        }

        private void DrawToggleMatrix()
        {
            var clickableLabelStyle = new GUIStyle(GUI.skin.label)
            {
                normal = { textColor = Color.cyan }
            };

            var r = GUILayoutUtility.GetRect(80 + longestReactorName + activators.Count * 15 + toggleSize,
                55 + reactors.Count * 30);

            GUIUtility.RotateAroundPivot(-90, new Vector2(r.x + longestReactorName, r.y));
            for (var i = 0; i < activators.Count; i++)
            {
                var activatorLabelContent = new GUIContent(activators[i].name);
                var labelSize             = clickableLabelStyle.CalcSize(activatorLabelContent);

                if (GUI.Button(new Rect(r.x, r.y + i * 15 + 3, labelSize.x, labelSize.y), activatorLabelContent,
                        clickableLabelStyle))
                {
                    Selection.activeGameObject = activators[i].gameObject;
                    EditorGUIUtility.PingObject(activators[i].gameObject);
                }
            }

            GUIUtility.RotateAroundPivot(90, new Vector2(r.x + longestReactorName, r.y));

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Activation Signal Subscribers", EditorStyles.boldLabel);
            DrawReactorToggles(enableFieldsArray, " ");

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Deactivation Signal Subscribers", EditorStyles.boldLabel);
            DrawReactorToggles(disableFieldsArray, " ");
        }


        private void DrawReactorToggles(bool[,] fieldsArray, string labelPrefix)
        {
            for (var j = 0; j < reactors.Count; j++)
            {
                EditorGUILayout.BeginHorizontal();

                var reactorLabelContent = new GUIContent(labelPrefix + " " + reactors[j].name);

                var labelSize = GUI.skin.label.CalcSize(reactorLabelContent);

                var clickableLabelStyle = new GUIStyle(GUI.skin.label)
                {
                    normal = { textColor = Color.cyan }
                };

                if (GUILayout.Button(reactorLabelContent, clickableLabelStyle, GUILayout.Width(150),
                        GUILayout.Height(labelSize.y)))
                    if (reactors[j] != null)
                    {
                        Selection.activeGameObject = reactors[j].gameObject;
                        EditorGUIUtility.PingObject(reactors[j].gameObject);
                    }

                for (var i = 0; i < activators.Count; i++)
                    fieldsArray[j, i] = EditorGUILayout.Toggle(fieldsArray[j, i], GUILayout.Width(toggleSize));

                EditorGUILayout.EndHorizontal();
            }
        }
    }
}