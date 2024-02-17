using UnityEngine;

namespace PuzzleForge
{
    [IconAttribute(@"Packages/com.waltergordy.puzzleforge/Editor/Resources/PuzzleForgeTriggerSignal.png")]
    public class PuzzleForgeRoomController : MonoBehaviour
    {
        [System.NonSerialized]
        public PuzzleForgeReactor[] objectsToTrigger;
        [System.NonSerialized]
        public PuzzleForgeSignalBase[] objectsWhichReact;
        private IPuzzleForgeReactor[] reactors;

        public void Awake()
        {
            GetReactors();
            GetTriggers();
            reactors = GetComponentsInChildren<IPuzzleForgeReactor>();
        }

        public PuzzleForgeReactor[] GetReactors()
        {
            objectsToTrigger = gameObject.GetComponentsInChildren<PuzzleForgeReactor>();

            return objectsToTrigger;
        }

        public PuzzleForgeSignalBase[] GetTriggers()
        {
            objectsWhichReact = gameObject.GetComponentsInChildren<PuzzleForgeSignalBase>();
            return objectsWhichReact;
        }

        public void Enable(ulong actionMask)
        {
            Debug.Log($"Enable received for layer {actionMask}");
            foreach (var reactor in reactors)
            {
                reactor.Activate(actionMask);
            }
        }

        public void Disable(ulong actionMask)
        {
            Debug.Log($"Disable received for layer {actionMask}");
            foreach (var reactor in reactors)
            {
                reactor.Deactivate(actionMask);
            }
        }
    }
}