using UnityEngine;

namespace PuzzleForge
{
    /// <summary>
    /// Creates a trigger that can be used to send signals to reactors directly
    /// </summary>
    public class PFInteractionTrigger : PFSignalBase
    {
        // Hide RoomController from the inspector
        [HideInInspector]
        public PFRoomController parentController;
        
        public PFSignalBase signal;
        public PFReactorBase reactor;
        public bool ingress;
        
        public void SendSignal()
        {
            reactor.React(ingress, signal);
        }
        
        private void OnTriggerEnter(Collider other)
        {
            SendSignal();
        } 
    }
}
