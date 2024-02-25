using UnityEngine;

namespace PuzzleForge
{
    /// <summary>
    /// PFTriggerSignal class for handling trigger signals.
    /// </summary>
    public class PFTriggerSignal : PFSignalEventBase
    {
        /// <summary>
        /// Called when a Rigidbody or a Collider enters the trigger.
        /// </summary>
        /// <param name="other">The other Collider involved in this collision.</param>
        private void OnTriggerEnter(Collider other)
        {
            SendSignal(other, true);
        }

        /// <summary>
        /// Called when a 2D collider enters a trigger.
        /// </summary>
        /// <param name="other">The collider that entered the trigger.</param>
        private void OnTriggerEnter2D(Collider2D other)
        {
            SendSignal(other, true);
        }

        /// <summary>
        /// Executes when the collider exits the trigger.
        /// </summary>
        /// <param name="other">The collider that exited the trigger.</param>
        private void OnTriggerExit(Collider other)
        {
            if (triggerType == TriggerType.Toggle)
                return;
            SendSignal(other, false);
        }

        /// <summary>
        /// Called when another Collider2D has stopped touching this trigger.
        /// </summary>
        /// <param name="other">The other Collider2D involved in this event.</param>
        private void OnTriggerExit2D(Collider2D other)
        {
            if (triggerType == TriggerType.Toggle)
                return;
            SendSignal(other, false);
        }
    }
}