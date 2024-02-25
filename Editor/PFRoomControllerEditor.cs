using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace PuzzleForge
{
    [CustomEditor(typeof(PFRoomController))]
    public class PFRoomControllerEditor : Editor
    {
        private readonly int longestReactorName = 150;

        private readonly int toggleSize = 15;
        private List<PFSignalEventBase> activators;
        private bool[,] deactivationDeactivateHookupsFieldsArray = new bool[4, 4];
        private bool[,] activationActivateHookupsFieldsArray = new bool[4, 4];
        private bool[,] deactivationActivateHookupsFieldsArray = new bool[4, 4];
        private bool[,] activationDeactivateHookupsFieldsArray = new bool[4, 4];
        private bool[,] activationResetHookupsFieldsArray = new bool[4, 4];
        private bool[,] deactivationResetHookupsFieldsArray = new bool[4, 4];

        private List<PFReactorBase> reactors;

        private PFRoomController switchRoomController;

        public void OnSceneGUI()
        {
            if (switchRoomController == null) return;

            foreach (var hookup in switchRoomController.OnActivationActivateHookups)
            {
                if (hookup == null) continue;

                foreach (var reactor in hookup.reactors)
                {
                    if (hookup.signal == null || reactor == null) continue;
                    EditorHelper.DrawCurve(hookup.signal.transform.position, reactor.transform.position, Color.red,
                        0.7f);
                }
            }

            foreach (var hookup in switchRoomController.OnDeactivationDeactivateHookups)
            {
                if (hookup == null) continue;

                foreach (var reactor in hookup.reactors)
                {
                    if (hookup.signal == null || reactor == null) continue;
                    EditorHelper.DrawCurve(hookup.signal.transform.position, reactor.transform.position, Color.red,
                        0.7f);
                }
            }
            
            foreach(var hookup in switchRoomController.OnActivationResetHookups)
            {
                if (hookup == null) continue;

                foreach (var reactor in hookup.reactors)
                {
                    if (hookup.signal == null || reactor == null) continue;
                    EditorHelper.DrawCurve(hookup.signal.transform.position, reactor.transform.position, Color.red,
                        0.7f);
                }
            }
        }

        public override void OnInspectorGUI()
        {
             DrawDefaultInspector();

            switchRoomController = (PFRoomController)target;
            
            activators = switchRoomController.signals;
            reactors = switchRoomController.reactors;

            // sanitize activators and reactors by remove nulls and updating the lists
            activators.RemoveAll(item => item == null);
            reactors.RemoveAll(item => item == null);
            
            
            activationActivateHookupsFieldsArray = new bool[reactors.Count, activators.Count];
            deactivationDeactivateHookupsFieldsArray = new bool[reactors.Count, activators.Count];
            activationDeactivateHookupsFieldsArray = new bool[reactors.Count, activators.Count];
            deactivationActivateHookupsFieldsArray = new bool[reactors.Count, activators.Count];
            activationResetHookupsFieldsArray = new bool[reactors.Count, activators.Count];
            deactivationResetHookupsFieldsArray = new bool[reactors.Count, activators.Count];

            InitializeToggleStates();

            EditorGUI.BeginChangeCheck();

            DrawToggleMatrix();

            if (EditorGUI.EndChangeCheck())
            {
                switchRoomController.OnActivationActivateHookups.Clear();
                switchRoomController.OnDeactivationDeactivateHookups.Clear();
                switchRoomController.OnActivationDeactivateHookups.Clear();
                switchRoomController.OnDeactivationActivateHookups.Clear();
                switchRoomController.OnActivationResetHookups.Clear();
                switchRoomController.OnDeactivationResetHookups.Clear();

                for (var activatorIndex = 0; activatorIndex < activators.Count; activatorIndex++)
                {
                    var onActivationActivateEntry = new PFActivationHookupEntry
                        { signal = activators[activatorIndex], reactors = new List<PFReactorBase>() };
                    var OnDeactivationDeactivateEntry = new PFActivationHookupEntry
                        { signal = activators[activatorIndex], reactors = new List<PFReactorBase>() };
                    var OnActivationDeactivateEntry = new PFActivationHookupEntry
                        { signal = activators[activatorIndex], reactors = new List<PFReactorBase>() };
                    var OnDeactivationActivateEntry = new PFActivationHookupEntry
                        { signal = activators[activatorIndex], reactors = new List<PFReactorBase>() };
                    var OnActivationResetEntry = new PFActivationHookupEntry
                        { signal = activators[activatorIndex], reactors = new List<PFReactorBase>() };
                    var OnDeactivationResetEntry = new PFActivationHookupEntry
                        { signal = activators[activatorIndex], reactors = new List<PFReactorBase>() };
                    

                    for (var reactorIndex = 0; reactorIndex < reactors.Count; reactorIndex++)
                    {
                        if (activationActivateHookupsFieldsArray[reactorIndex, activatorIndex])
                            onActivationActivateEntry.reactors.Add(reactors[reactorIndex]);

                        if (deactivationDeactivateHookupsFieldsArray[reactorIndex, activatorIndex])
                            OnDeactivationDeactivateEntry.reactors.Add(reactors[reactorIndex]);

                        if (activationDeactivateHookupsFieldsArray[reactorIndex, activatorIndex])
                            OnActivationDeactivateEntry.reactors.Add(reactors[reactorIndex]);

                        if (deactivationActivateHookupsFieldsArray[reactorIndex, activatorIndex])
                            OnDeactivationActivateEntry.reactors.Add(reactors[reactorIndex]);
                        
                        if (activationResetHookupsFieldsArray[reactorIndex, activatorIndex])
                            OnActivationResetEntry.reactors.Add(reactors[reactorIndex]);
                        
                        if (deactivationResetHookupsFieldsArray[reactorIndex, activatorIndex])
                            OnDeactivationResetEntry.reactors.Add(reactors[reactorIndex]);
                    }

                    if (onActivationActivateEntry.reactors.Count > 0) 
                        switchRoomController.OnActivationActivateHookups.Add(onActivationActivateEntry);

                    if (OnDeactivationDeactivateEntry.reactors.Count > 0)
                        switchRoomController.OnDeactivationDeactivateHookups.Add(OnDeactivationDeactivateEntry);
                    
                    if (OnActivationDeactivateEntry.reactors.Count > 0)
                        switchRoomController.OnActivationDeactivateHookups.Add(OnActivationDeactivateEntry);
                    
                    if(OnDeactivationActivateEntry.reactors.Count > 0)
                        switchRoomController.OnDeactivationActivateHookups.Add(OnDeactivationActivateEntry);
                    
                    if(OnActivationResetEntry.reactors.Count > 0)
                        switchRoomController.OnActivationResetHookups.Add(OnActivationResetEntry);
                    
                    if(OnDeactivationResetEntry.reactors.Count > 0)
                        switchRoomController.OnDeactivationResetHookups.Add(OnDeactivationResetEntry);
                }

                EditorUtility.SetDirty(target);
            }
        }

        private void InitializeToggleStates()
        {
            for (var j = 0; j < reactors.Count; j++)
                for (var i = 0; i < activators.Count; i++)
                {
                    activationActivateHookupsFieldsArray[j, i] = false;
                    deactivationDeactivateHookupsFieldsArray[j, i] = false;
                    deactivationActivateHookupsFieldsArray[j, i] = false;
                    activationDeactivateHookupsFieldsArray[j, i] = false;
                    activationResetHookupsFieldsArray[j, i] = false;
                    deactivationResetHookupsFieldsArray[j, i] = false;
                }

            foreach (var entry in switchRoomController.OnActivationActivateHookups)
            {
                var activatorIndex = activators.IndexOf(entry.signal);
                if (activatorIndex != -1)
                    foreach (var reactor in entry.reactors)
                    {
                        var reactorIndex = reactors.IndexOf(reactor);
                        if (reactorIndex != -1) activationActivateHookupsFieldsArray[reactorIndex, activatorIndex] = true;
                    }
            }

            foreach (var entry in switchRoomController.OnDeactivationDeactivateHookups)
            {
                var activatorIndex = activators.IndexOf(entry.signal);
                if (activatorIndex != -1)
                    foreach (var reactor in entry.reactors)
                    {
                        var reactorIndex = reactors.IndexOf(reactor);
                        if (reactorIndex != -1) deactivationDeactivateHookupsFieldsArray[reactorIndex, activatorIndex] = true;
                    }
            }
            
            foreach (var entry in switchRoomController.OnDeactivationActivateHookups)
            {
                var activatorIndex = activators.IndexOf(entry.signal);
                if (activatorIndex != -1)
                    foreach (var reactor in entry.reactors)
                    {
                        var reactorIndex = reactors.IndexOf(reactor);
                        if (reactorIndex != -1) deactivationActivateHookupsFieldsArray[reactorIndex, activatorIndex] = true;
                    }
            }
            
            foreach (var entry in switchRoomController.OnActivationDeactivateHookups)
            {
                var activatorIndex = activators.IndexOf(entry.signal);
                if (activatorIndex != -1)
                    foreach (var reactor in entry.reactors)
                    {
                        var reactorIndex = reactors.IndexOf(reactor);
                        if (reactorIndex != -1) activationDeactivateHookupsFieldsArray[reactorIndex, activatorIndex] = true;
                    }
            }
            
            foreach (var entry in switchRoomController.OnActivationResetHookups)
            {
                var activatorIndex = activators.IndexOf(entry.signal);
                if (activatorIndex != -1)
                    foreach (var reactor in entry.reactors)
                    {
                        var reactorIndex = reactors.IndexOf(reactor);
                        if (reactorIndex != -1) activationResetHookupsFieldsArray[reactorIndex, activatorIndex] = true;
                    }
            }
            
            foreach (var entry in switchRoomController.OnDeactivationResetHookups)
            {
                var activatorIndex = activators.IndexOf(entry.signal);
                if (activatorIndex != -1)
                    foreach (var reactor in entry.reactors)
                    {
                        var reactorIndex = reactors.IndexOf(reactor);
                        if (reactorIndex != -1) deactivationResetHookupsFieldsArray[reactorIndex, activatorIndex] = true;
                    }
            }
            
        }

        private void DrawToggleMatrix()
        {
            var clickableLabelStyle = new GUIStyle(GUI.skin.label)
            {
                normal = { textColor = Color.cyan }
            };

            var r = GUILayoutUtility.GetRect(activators.Count * 15 + toggleSize,
                150);

            GUIUtility.RotateAroundPivot(-90, new Vector2(r.x + longestReactorName, r.y));
            EditorGUILayout.LabelField("Activators", EditorStyles.boldLabel);
            for (var i = 0; i < activators.Count; i++)
            {
                var activatorLabelContent = new GUIContent(activators[i].name);
                var labelSize = clickableLabelStyle.CalcSize(activatorLabelContent);

                if (GUI.Button(new Rect(r.x, r.y + i * 18 + 3, labelSize.x, labelSize.y), activatorLabelContent,
                        clickableLabelStyle))
                {
                    Selection.activeGameObject = activators[i].gameObject;
                    EditorGUIUtility.PingObject(activators[i].gameObject);
                }
            }

            GUIUtility.RotateAroundPivot(90, new Vector2(r.x + longestReactorName, r.y));

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("On Activation Activate Signal Subscribers", EditorStyles.boldLabel);
            DrawReactorToggles(activationActivateHookupsFieldsArray, " ");

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("On Deactivation Deactivate Signal Subscribers", EditorStyles.boldLabel);
            DrawReactorToggles(deactivationDeactivateHookupsFieldsArray, " ");
            
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("On Activation Deactivate Signal Subscribers", EditorStyles.boldLabel);
            DrawReactorToggles(activationDeactivateHookupsFieldsArray, " ");
            
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("On Deactivation Activate Signal Subscribers", EditorStyles.boldLabel);
            DrawReactorToggles(deactivationActivateHookupsFieldsArray, " ");
            
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("On Activation Reset Signal Subscribers", EditorStyles.boldLabel);
            DrawReactorToggles(activationResetHookupsFieldsArray, " ");
            
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("On Deactivation Reset Signal Subscribers", EditorStyles.boldLabel);
            DrawReactorToggles(deactivationResetHookupsFieldsArray, " ");
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