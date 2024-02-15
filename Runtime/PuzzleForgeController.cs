using System;
using UnityEngine;
using System.Collections;
using UnityEditor;


namespace PuzzleForge
{
    public class PuzzleForgeController : MonoBehaviour
    {
        [System.NonSerialized]
        public PuzzleForgeReactor[] objectsToActivate;
        [System.NonSerialized]
        public PuzzleForgeActivator[] objectsWhichActivate;
        private IPuzzleForgeActivatable[] activatables;

        public void Awake()
        {
            GetReactors();
            GetActivators();
            activatables = GetComponentsInChildren<IPuzzleForgeActivatable>();
        }



        public PuzzleForgeReactor[] GetReactors()
        {
            objectsToActivate = gameObject.GetComponentsInChildren<PuzzleForgeReactor>();

            return objectsToActivate;
        }

        public PuzzleForgeActivator[] GetActivators()
        {
            objectsWhichActivate = gameObject.GetComponentsInChildren<PuzzleForgeActivator>();
            return objectsWhichActivate;
        }

        public void Enable(ulong actionMask)
        {
            Debug.Log($"Enable recived for layer {actionMask}");
            foreach (var activatable in activatables)
            {
                activatable.Activate(actionMask);
            }
        }

        public void Disable(ulong actionMask)
        {
            Debug.Log($"Disable recived for layer {actionMask}");
            foreach (var activatable in activatables)
            {
                activatable.Deactivate(actionMask);
            }
        }
    }
}