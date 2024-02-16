using UnityEngine;

namespace PuzzleForge
{
    public class PuzzleForgeRoomController : MonoBehaviour
    {
        [System.NonSerialized]
        public PuzzleForgeReactor[] objectsToTrigger;
        [System.NonSerialized]
        public PuzzleForgeTriggerSource[] objectsWhichReact;
        private IPuzzleForgeTriggerable[] triggers;

        public void Awake()
        {
            GetReactors();
            GetTriggers();
            triggers = GetComponentsInChildren<IPuzzleForgeTriggerable>();
        }

        public PuzzleForgeReactor[] GetReactors()
        {
            objectsToTrigger = gameObject.GetComponentsInChildren<PuzzleForgeReactor>();

            return objectsToTrigger;
        }

        public PuzzleForgeTriggerSource[] GetTriggers()
        {
            objectsWhichReact = gameObject.GetComponentsInChildren<PuzzleForgeTriggerSource>();
            return objectsWhichReact;
        }

        public void Enable(ulong actionMask)
        {
            Debug.Log($"Enable received for layer {actionMask}");
            foreach (var trigger in triggers)
            {
                trigger.Activate(actionMask);
            }
        }

        public void Disable(ulong actionMask)
        {
            Debug.Log($"Disable received for layer {actionMask}");
            foreach (var trigger in triggers)
            {
                trigger.Deactivate(actionMask);
            }
        }
    }
}