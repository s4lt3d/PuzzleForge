using UnityEngine;

namespace PuzzleForge
{
    public class PFTriggerSignal : PFSignalBase
    {
        private void OnTriggerEnter(Collider other)
        {
            SendSignal(other, true);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            SendSignal(other, true);
        }

        private void OnTriggerExit(Collider other)
        {
            SendSignal(other, false);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            SendSignal(other, false);
        }
    }
}