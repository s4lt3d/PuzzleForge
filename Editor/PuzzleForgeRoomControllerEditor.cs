using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace PuzzleForge
{
    [CustomEditor(typeof(PuzzleForgeRoomController))]
    public class PuzzleForgeRoomControllerEditor : Editor
    {
        PuzzleForgeRoomController switchRoomController;
        
        List<PuzzleForgeReactorBase> reactors;
        List<PuzzleForgeSignalBase> activators;
        
         bool[,] enableFieldsArray = new bool[4, 4];
         bool[,] disableFieldsArray = new bool[4, 4];
         
         int longestName = 150;
         int longestReactorName = 150;
        
         int toggleSize = 15;
        
        GameObject selected = null;

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            switchRoomController = (PuzzleForgeRoomController)target;
            activators = switchRoomController.signals;
            reactors = switchRoomController.reactors;

            // Resize the arrays based on current lists (optional, if list sizes can change)
            enableFieldsArray = new bool[reactors.Count, activators.Count];
            disableFieldsArray = new bool[reactors.Count, activators.Count];

            // Initialize toggle states based on current hookup lists
            InitializeToggleStates();

            EditorGUI.BeginChangeCheck(); // Start tracking changes

            DrawToggleMatrix();

            if (EditorGUI.EndChangeCheck()) // Check if any changes were made
            {
                // Clear existing entries to repopulate them based on toggles
                switchRoomController.activationHookups.Clear();
                switchRoomController.deactivationHookups.Clear();

                // Iterate through all activators and reactors to update hookups
                for (int activatorIndex = 0; activatorIndex < activators.Count; activatorIndex++)
                {
                    var activationEntry = new ActivationHookupEntry
                        { signal = activators[activatorIndex], reactors = new List<PuzzleForgeReactorBase>() };
                    var deactivationEntry = new ActivationHookupEntry
                        { signal = activators[activatorIndex], reactors = new List<PuzzleForgeReactorBase>() };

                    for (int reactorIndex = 0; reactorIndex < reactors.Count; reactorIndex++)
                    {
                        if (enableFieldsArray[reactorIndex, activatorIndex])
                        {
                            activationEntry.reactors.Add(reactors[reactorIndex]);
                        }

                        if (disableFieldsArray[reactorIndex, activatorIndex])
                        {
                            deactivationEntry.reactors.Add(reactors[reactorIndex]);
                        }
                    }

                    // Only add entries to the list if there are reactors assigned to them
                    if (activationEntry.reactors.Count > 0)
                    {
                        switchRoomController.activationHookups.Add(activationEntry);
                    }

                    if (deactivationEntry.reactors.Count > 0)
                    {
                        switchRoomController.deactivationHookups.Add(deactivationEntry);
                    }
                }

                EditorUtility.SetDirty(target);
            }
        }
        
        void InitializeToggleStates()
        {
            // Reset all toggle states to false initially
            for (int j = 0; j < reactors.Count; j++)
            {
                for (int i = 0; i < activators.Count; i++)
                {
                    enableFieldsArray[j, i] = false;
                    disableFieldsArray[j, i] = false;
                }
            }

            // Set enableFieldsArray based on activationHookups
            foreach (var entry in switchRoomController.activationHookups)
            {
                int activatorIndex = activators.IndexOf(entry.signal);
                if (activatorIndex != -1)
                {
                    foreach (var reactor in entry.reactors)
                    {
                        int reactorIndex = reactors.IndexOf(reactor);
                        if (reactorIndex != -1)
                        {
                            enableFieldsArray[reactorIndex, activatorIndex] = true;
                        }
                    }
                }
            }

            // Set disableFieldsArray based on deactivationHookups
            foreach (var entry in switchRoomController.deactivationHookups)
            {
                int activatorIndex = activators.IndexOf(entry.signal);
                if (activatorIndex != -1)
                {
                    foreach (var reactor in entry.reactors)
                    {
                        int reactorIndex = reactors.IndexOf(reactor);
                        if (reactorIndex != -1)
                        {
                            disableFieldsArray[reactorIndex, activatorIndex] = true;
                        }
                    }
                }
            }
        }
                    
        void DrawToggleMatrix()
        {
            // Assuming longestName is used for button width calculations
            // UpdateLongestNameWidths();
    
            GUIStyle clickableLabelStyle = new GUIStyle(GUI.skin.label)
            {
                // Set the text color to something more readable, e.g., cyan for better visibility
                normal = { textColor = Color.cyan }
            };

            Rect r = GUILayoutUtility.GetRect(80 + longestReactorName + activators.Count * 15 + toggleSize, 55 + reactors.Count * 30);

            // Use the custom style for drawing activator labels as clickable buttons
            GUIUtility.RotateAroundPivot(-90, new Vector2(r.x + longestReactorName, r.y));
            for (int i = 0; i < activators.Count; i++)
            {
                GUIContent activatorLabelContent = new GUIContent(activators[i].name);
                Vector2 labelSize = clickableLabelStyle.CalcSize(activatorLabelContent);

                // Adjust the button rect based on calculated label size for consistency
                if (GUI.Button(new Rect(r.x, r.y + i * 15 + 3, labelSize.x, labelSize.y), activatorLabelContent, clickableLabelStyle))
                {
                    // Select and ping the activator GameObject in the editor
                    Selection.activeGameObject = activators[i].gameObject;
                    EditorGUIUtility.PingObject(activators[i].gameObject);
                }
            }
            GUIUtility.RotateAroundPivot(90, new Vector2(r.x + longestReactorName, r.y));

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Activation Signal Subscribers", EditorStyles.boldLabel);
            DrawReactorToggles(enableFieldsArray, " ");

            // Section for "Disable" toggles
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Deactivation Signal Subscribers", EditorStyles.boldLabel);
            DrawReactorToggles(disableFieldsArray, " ");
        }

        
        
        void DrawReactorToggles(bool[,] fieldsArray, string labelPrefix)
        {
            for (int j = 0; j < reactors.Count; j++)
            {
                EditorGUILayout.BeginHorizontal();
        
                // Create a GUIContent for the label to make it clickable
                GUIContent reactorLabelContent = new GUIContent(labelPrefix + " " + reactors[j].name);
        
                // Calculate the size of the label to properly size the button
                Vector2 labelSize = GUI.skin.label.CalcSize(reactorLabelContent);
        
                GUIStyle clickableLabelStyle = new GUIStyle(GUI.skin.label)
                {
                    // Set the text color to something more readable, e.g., white
                    normal = { textColor = Color.cyan }
                };
                
                // Create a invisible button over the label. If clicked, it will select the reactor in the scene.
                if (GUILayout.Button(reactorLabelContent, clickableLabelStyle, GUILayout.Width(150), GUILayout.Height(labelSize.y)))
                {
                    // Focus the scene view camera on the reactor object
                    if (reactors[j] != null)
                    {
                        Selection.activeGameObject = reactors[j].gameObject;
                        EditorGUIUtility.PingObject(reactors[j].gameObject);
                    }
                }

                // Tooltips and Context Clicks (Optional)
                // You can add tooltips or context menu functionality here if needed.
        
                for (int i = 0; i < activators.Count; i++)
                {
                    fieldsArray[j, i] = EditorGUILayout.Toggle(fieldsArray[j, i], GUILayout.Width(toggleSize));
                }

                EditorGUILayout.EndHorizontal();
            }
        }
        
        
        
        
        
        private void UpdateLongestNameWidths()
        {
            GUIStyle labelStyle = GUI.skin.label;
            int minWidth = 100; // Minimum width to ensure UI usability

            longestName = minWidth;
            longestReactorName = minWidth;

            foreach (var activator in activators)
            {
                Vector2 size = labelStyle.CalcSize(new GUIContent(activator.name));
                if (size.x > longestName)
                {
                    longestName = (int)size.x;
                }
            }

            foreach (var reactor in reactors)
            {
                Vector2 size = labelStyle.CalcSize(new GUIContent(reactor.name));
                if (size.x > longestReactorName)
                {
                    longestReactorName = (int)size.x;
                }
            }

            // Adjust for additional UI elements or padding as necessary
            longestName += 20; // Add padding or space for toggle, etc.
            longestReactorName += 20;
        }

    }
}








// using UnityEngine;
// using UnityEditor;
// using System;
// using System.Collections.Generic;
//
// namespace PuzzleForge
// {
//     [CustomEditor(typeof(PuzzleForgeRoomController))]
//     public class PuzzleForgeControllerEditor : Editor
//     {
//         private PuzzleForgeRoomController selectedObject;
//         bool[,] enableFieldsArray = new bool[4, 4];
//         bool[,] disableFieldsArray = new bool[4, 4];
//         PuzzleForgeReactor[] reactors;
//         PuzzleForgeTriggerSignal[] activators;
//         GameObject selected = null;
//
//         PuzzleForgeRoomController switchRoomController;
//
//         private void DrawCurve(Vector3 pos1, Vector3 pos2, Color color, float curveRatio, int width=1)
//         {
//             Vector3 start = pos1;
//
//             Vector3 end = pos2;
//
//             float offset = Mathf.Abs(start.y - end.y) * curveRatio;
//
//             Handles.DrawBezier(
//                 start,
//                 end,
//                 start - Vector3.up * offset,
//                 end + Vector3.up * offset,
//                 color,
//                 EditorGUIUtility.whiteTexture,
//                 width
//             );
//         }
//
//         public void OnEnable()
//         {
//             if (target != null)
//                 selectedObject = target as PuzzleForgeRoomController;
//             reactors = selectedObject.GetReactors();
//             activators = selectedObject.GetTriggers();
//             GetActivationMasks();
//         }
//
//         public void OnSceneGUI()
//         {
//             //    Event.current.Use();
//
//             if (selected != null)
//                 Handles.Label(selected.transform.position + Vector3.up * 2, selected.name);
//
//
//             switchRoomController = target as PuzzleForgeRoomController;
//             if (switchRoomController == null) return;
//
//             // Example drawing a wire cube around the SwitchController object
//             Handles.DrawWireCube(switchRoomController.transform.position, Vector3.one * 1.5f);
//
//             // Drawing curves to reactors and activators, assuming these methods are correctly implemented
//             foreach (PuzzleForgeReactor reactor in switchRoomController.objectsToTrigger)
//             {
//                 DrawCurve(switchRoomController.transform.position, reactor.transform.position, Color.gray, 0.85f);
//             }
//
//             foreach (PuzzleForgeTriggerSignal activator in switchRoomController.objectsWhichReact)
//             {
//                 DrawCurve(switchRoomController.transform.position, activator.transform.position, Color.gray, 0.7f);
//             }
//
//
//             // Drawing curves to reactors and activators, assuming these methods are correctly implemented
//             foreach (PuzzleForgeReactor reactor in switchRoomController.objectsToTrigger)
//             {
//                 List<PuzzleForgeTriggerSignal> activators = GetActivators(reactor);
//                 foreach(PuzzleForgeTriggerSignal activator in activators)
//                 {
//                     DrawCurve(activator.transform.position, reactor.transform.position, Color.red, 0.85f, 3);
//                 }
//             }
//         }
//
//         private List<PuzzleForgeTriggerSignal> GetActivators(PuzzleForgeReactor reactor)
//         {
//             List<PuzzleForgeTriggerSignal> activators  = new List<PuzzleForgeTriggerSignal>();
//
//             PuzzleForgeTriggerSignal[] a = switchRoomController.GetTriggers();
//
//             ulong activationMask = reactor.disableMask | reactor.enableMask;
//
//             foreach (PuzzleForgeTriggerSignal activator in a) { 
//            //     if((activator.activationID & activationMask) > 0)
//            //         activators.Add(activator);
//             }
//
//
//
//             return activators;
//         }
//
//         public override void OnInspectorGUI()
//         {
//
//             DrawDefaultInspector();
//             switchRoomController = (PuzzleForgeRoomController)target;
//
//             if (selectedObject == null)
//                 return;
//
//             reactors = selectedObject.GetReactors();
//             activators = selectedObject.GetTriggers();
//
//             DrawToggleMatrix();
//         }
//
//         void DrawToggleMatrix()
//         {
//             int longestName = 150;
//             int longestReactorName = 150;
//
//             Rect r = GUILayoutUtility.GetRect(longestReactorName + activators.Length * 15, longestName + reactors.Length * 30 + 30);
//
//             GUIUtility.RotateAroundPivot(-90, new Vector2(r.x + longestReactorName, r.y));
//             for (int i = 0; i < activators.Length; i++)
//             {
//                 //         EditorGUI.LabelField(new Rect(r.x, r.y + i * 15, longestName, 15), reactors[i].name);
//                 if (GUI.Button(new Rect(r.x + longestReactorName - longestName, r.y + i * 15, longestName, 15), activators[i].name, GUI.skin.label))
//                 {
//                     selected = activators[i].gameObject;
//                 }
//             }
//             GUIUtility.RotateAroundPivot(90, new Vector2(r.x + longestReactorName, r.y));
//
//             for (int j = 0; j < reactors.Length; j++)
//             {
//                 EditorGUI.LabelField(new Rect(r.x, r.y + j * 30 + longestName, longestReactorName, 15), "Enable " + reactors[j].name);
//                 EditorGUI.LabelField(new Rect(r.x, r.y + j * 30 + longestName + 15, longestReactorName, 15), "Disable " + reactors[j].name);
//
//                 for (int i = 0; i < activators.Length; i++)
//                 {
//                     enableFieldsArray[j, i] = EditorGUI.Toggle(new Rect(r.x + i * 15 + longestReactorName, r.y + j * 30 + longestName, 15, 15), enableFieldsArray[j, i]);
//                 }
//
//                 for (int i = 0; i < activators.Length; i++)
//                 {
//                     disableFieldsArray[j, i] = EditorGUI.Toggle(new Rect(r.x + i * 15 + longestReactorName, r.y + j * 30 + longestName + 15, 15, 15), disableFieldsArray[j, i]);
//                 }
//             }
//
//             if (GUI.changed)
//             {
//                 SetActivationMasks();
//             }
//         }
//
//         private void SetActivationMasks()
//         {
//             for (int i = 0; i < reactors.Length; i++)
//             {
//                 ulong activationMask = 0;
//                 ulong deactivationMask = 0;
//
//                 for (int j = 0; j < activators.Length; j++)
//                 {
//                     if (enableFieldsArray[i, j] == true)
//                     {
//                         activationMask |= (ulong)1 << j;
//                     }
//                     if (disableFieldsArray[i, j] == true)
//                     {
//                         deactivationMask |= (ulong)1 << j;
//                     }
//                 }
//                 reactors[i].enableMask = activationMask;
//                 reactors[i].disableMask = deactivationMask;
//                 EditorUtility.SetDirty(reactors[i]);
//             }
//
//             for (int j = 0; j < activators.Length; j++)
//             {
//                 activators[j].activationID = (ulong)1 << j;
//                 EditorUtility.SetDirty(activators[j]);
//             }
//         }
//
//         private void GetActivationMasks()
//         {
//             enableFieldsArray = new bool[reactors.Length, activators.Length];
//             disableFieldsArray = new bool[reactors.Length, activators.Length];
//
//             for (int i = 0; i < reactors.Length; i++)
//             {
//                 for (int j = 0; j < activators.Length; j++)
//                 {
//                     if ((reactors[i].enableMask & ((ulong)1 << j)) > 0)
//                     {
//                         enableFieldsArray[i, j] = true;
//                     }
//                     else
//                         enableFieldsArray[i, j] = false;
//
//                     if ((reactors[i].disableMask & ((ulong)1 << j)) > 0)
//                     {
//                         disableFieldsArray[i, j] = true;
//                     }
//                     else
//                         disableFieldsArray[i, j] = false;
//                 }
//             }
//         }
//     }
// }