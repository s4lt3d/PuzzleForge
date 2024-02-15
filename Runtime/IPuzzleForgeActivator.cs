using UnityEngine;

namespace PuzzleForge
{
    interface IPuzzleForgeActivator
    {
        public void StartInteraction(Component other);
        public void StopInteraction(Component other);
    }
}