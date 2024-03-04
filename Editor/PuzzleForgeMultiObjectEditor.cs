using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

namespace PuzzleForge
{
    public class PuzzleForgeMultiObjectEditor : EditorWindow
    {
        private readonly int toggleSize = 15;
        private readonly int longestReactorName = 150;
        private List<PFSignalEventBase> activators;
        private List<PFReactorBase> reactors;
        private bool[,] deactivationDeactivateHookupsFieldsArray = new bool[4, 4];
        private bool[,] activationActivateHookupsFieldsArray = new bool[4, 4];
        private bool[,] deactivationActivateHookupsFieldsArray = new bool[4, 4];
        private bool[,] activationDeactivateHookupsFieldsArray = new bool[4, 4];
        private bool[,] activationResetHookupsFieldsArray = new bool[4, 4];
        private bool[,] deactivationResetHookupsFieldsArray = new bool[4, 4];
        bool showGroup1 = true;
        bool showGroup2 = false;
        bool showGroup3 = false;
        bool showGroup4 = false;
        bool showGroup5 = false;
        bool showGroup6 = false;
        
        private Vector2 scrollPosition; // For tracking the scroll position

        public static void ShowWindow()
        {
            // Show existing window instance. If one doesn't exist, make one.
            GetWindow(typeof(PuzzleForgeMultiObjectEditor), false);
        }
        
        [MenuItem("Window/Puzzle Forge Configuration Wizard")]
        static void Init()
        {
            PuzzleForgeMultiObjectEditor window = (PuzzleForgeMultiObjectEditor)EditorWindow.GetWindow(typeof(PuzzleForgeMultiObjectEditor));
            window.Show();

            HashSet<PFSignalEventBase> activators = new HashSet<PFSignalEventBase>();
            HashSet<PFReactorBase> reactors = new HashSet<PFReactorBase>();
            
            foreach (var obj in Selection.objects)
            {
                if(obj.GetComponent<PFSignalEventBase>() != null)
                {
                    activators.Add(obj.GetComponent<PFSignalEventBase>());
                    foreach(var r in obj.GetComponent<PFSignalEventBase>().GetAllReactors())
                    {
                        reactors.Add(r);
                    }
                }
                
                if(obj.GetComponent<PFReactorBase>() != null)
                {
                    reactors.Add(obj.GetComponent<PFReactorBase>());

                }
            }
            
            window.activators = new List<PFSignalEventBase>(activators);
            window.reactors = new List<PFReactorBase>(reactors);
            window.activators.Sort( (a, b) => String.Compare(a.name, b.name, StringComparison.Ordinal));
            window.reactors.Sort( (a, b) => String.Compare(a.name, b.name, StringComparison.Ordinal));
            
            window.activationActivateHookupsFieldsArray = new bool[window.reactors.Count, window.activators.Count];
            window.deactivationDeactivateHookupsFieldsArray = new bool[window.reactors.Count, window.activators.Count];
            window.activationDeactivateHookupsFieldsArray = new bool[window.reactors.Count, window.activators.Count];
            window.deactivationActivateHookupsFieldsArray = new bool[window.reactors.Count, window.activators.Count];
            window.activationResetHookupsFieldsArray = new bool[window.reactors.Count, window.activators.Count];
            window.deactivationResetHookupsFieldsArray = new bool[window.reactors.Count, window.activators.Count];
            
            window.InitializeToggleStates();
            
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
            
            
            showGroup1 = EditorGUILayout.Foldout(showGroup1, "On Activation Activate Signal Subscribers");
            if(showGroup1)
                DrawReactorToggles(activationActivateHookupsFieldsArray, " ");
            
            showGroup2 = EditorGUILayout.Foldout(showGroup2, "On Deactivation Deactivate Signal Subscribers");
            if(showGroup2)
                DrawReactorToggles(deactivationDeactivateHookupsFieldsArray, " ");
            
            showGroup3 = EditorGUILayout.Foldout(showGroup3, "On Deactivation Activate Signal Subscribers");
            if(showGroup3)
                DrawReactorToggles(deactivationActivateHookupsFieldsArray, " ");
            
            showGroup4 = EditorGUILayout.Foldout(showGroup4, "On Activation Deactivate Signal Subscribers");
            if(showGroup4)
                DrawReactorToggles(activationDeactivateHookupsFieldsArray, " ");
            
            showGroup5 = EditorGUILayout.Foldout(showGroup5, "On Activation Reset Signal Subscribers");
            if(showGroup5)
                DrawReactorToggles(activationResetHookupsFieldsArray, " ");
            
            showGroup6 = EditorGUILayout.Foldout(showGroup6, "On Deactivation Reset Signal Subscribers");
            if(showGroup6)
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

            
            foreach (var activator in activators)
            {
                PopulateFieldsArray(activator, activator.OnActivationActivateHookups, activationActivateHookupsFieldsArray);
                PopulateFieldsArray(activator, activator.OnDeactivationDeactivateHookups, deactivationDeactivateHookupsFieldsArray);
                PopulateFieldsArray(activator, activator.OnDeactivationActivateHookups, deactivationActivateHookupsFieldsArray);
                PopulateFieldsArray(activator, activator.OnActivationDeactivateHookups, activationDeactivateHookupsFieldsArray);
                PopulateFieldsArray(activator, activator.OnActivationResetHookups, activationResetHookupsFieldsArray);
                PopulateFieldsArray(activator, activator.OnDeactivationResetHookups, deactivationResetHookupsFieldsArray);
            }
        }

        private void PopulateFieldsArray(PFSignalEventBase activator, List<PFReactorBase> connections, bool[,] hookups)
        {
            var activatorIndex = activators.IndexOf(activator);
            if (activatorIndex != -1)
            {
                foreach (var reactor in reactors)
                {
                    var reactorIndex = reactors.IndexOf(reactor);
                    if(connections.Contains(reactor))
                        if (reactorIndex != -1)
                            hookups[reactorIndex, activatorIndex] = true;
                }
            }
        }

        void OnGUI()
        {
            

            if(Selection.objects.Length == 0)
            {
                scrollPosition = Vector2.zero;
                EditorGUILayout.HelpBox("No object selected. Please select an object to configure.", MessageType.Error);
                return;
            }
            scrollPosition = GUILayout.BeginScrollView(scrollPosition);
            
            SetupConfiguration();
            
            GUILayout.EndScrollView();
        }

        void SetupConfiguration()
        {
            bool shouldApply = false;
            shouldApply = GUILayout.Button("Apply Configuration");
                
            DrawToggleMatrix();
            ApplyChanges(shouldApply);
        }

        private void ApplyChanges(bool shouldApply)
        {
            if (shouldApply)
            {
                for(int i = 0; i < activators.Count; i++)
                {
                    PopulateHookups(activators[i].OnActivationActivateHookups, activationActivateHookupsFieldsArray, i);
                    PopulateHookups(activators[i].OnDeactivationDeactivateHookups, deactivationDeactivateHookupsFieldsArray, i);
                    PopulateHookups(activators[i].OnDeactivationActivateHookups, deactivationActivateHookupsFieldsArray, i);
                    PopulateHookups(activators[i].OnActivationDeactivateHookups, activationDeactivateHookupsFieldsArray, i);
                    PopulateHookups(activators[i].OnActivationResetHookups, activationResetHookupsFieldsArray, i);
                    PopulateHookups(activators[i].OnDeactivationResetHookups, deactivationResetHookupsFieldsArray, i);
                }

                foreach (var obj in Selection.objects)
                {
                    if(obj.GetComponent<PFSignalEventBase>())
                        EditorUtility.SetDirty(obj.GetComponent<PFSignalEventBase>());
                    if(obj.GetComponent<PFReactorBase>())
                        EditorUtility.SetDirty(obj.GetComponent<PFReactorBase>());
                }

                EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
                AssetDatabase.SaveAssets();
            }
        }

        private void PopulateHookups(List<PFReactorBase> hookupsList, bool[,] hookupArray, int i)
        {
            hookupsList.Clear();
            for(int j = 0; j < reactors.Count; j++)
                if(hookupArray[j, i])
                    hookupsList.Add(reactors[j]);
        }

        void OnSelectionChange() 
        {

            Init();

            Repaint();
        }
    }
}