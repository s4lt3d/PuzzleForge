using UnityEngine;
using UnityEditor;

namespace PuzzleForge
{


    public class ConfigurationWizard : EditorWindow
    {
        private string myString = "Hello World";
        private bool groupEnabled;
        private bool myBool = true;
        private float myFloat = 1.23f;

        // Add menu named "My Window" to the Window menu
        [MenuItem("Window/Configuration Wizard")]
        static void Init()
        {
            // Get existing open window or if none, make a new one:
            ConfigurationWizard window = (ConfigurationWizard)EditorWindow.GetWindow(typeof(ConfigurationWizard));
            window.Show();
        }

        void OnGUI()
        {
            GUILayout.Label("Base Settings", EditorStyles.boldLabel);
            myString = EditorGUILayout.TextField("Text Field", myString);

            groupEnabled = EditorGUILayout.BeginToggleGroup("Optional Settings", groupEnabled);
            myBool = EditorGUILayout.Toggle("Toggle", myBool);
            myFloat = EditorGUILayout.Slider("Slider", myFloat, -3, 3);
            EditorGUILayout.EndToggleGroup();

            // Implement your configuration setup logic here
            // For example, a button to finalize the configuration
            if (GUILayout.Button("Setup Configuration"))
            {
                SetupConfiguration();
            }
        }

        void SetupConfiguration()
        {
            // Your configuration setup logic here
            Debug.Log("Configuration Setup Complete!");
        }

        // Respond to selection change in the editor
        void OnSelectionChange() 
        {
            // Show the window if more than one object is selected
            if (Selection.objects.Length > 1)
            {
                Init();
            }
            else
            {
                // Optionally, close the window if the selection does not meet criteria
                this.Close();
            }
        }
    }
}