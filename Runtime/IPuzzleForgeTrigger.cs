using UnityEngine;

namespace PuzzleForge
{
    interface IPuzzleForgeTrigger
    {
        public void StartInteraction(Component other);
        public void StopInteraction(Component other);
    }
}