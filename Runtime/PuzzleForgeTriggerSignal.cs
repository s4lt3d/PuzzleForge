using UnityEngine;

namespace PuzzleForge
{
    public class PuzzleForgeTriggerSignal : PuzzleForgeSignalBase
    {
        void OnTriggerEnter2D(Collider2D other)
        {
            SendSignal(other, true);
        }

        void OnTriggerEnter(Collider other)
        {
            SendSignal(other, true);
        }

        void OnTriggerExit(Collider other)
        {
            SendSignal(other, false);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            SendSignal(other, false);
        }
    }
}