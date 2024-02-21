using System.Collections.Generic;
using UnityEngine;

namespace PuzzleForge
{
    /// <summary>
    /// This class is a controller for a room, it is responsible for managing signals and reactors
    /// It is also responsible for sending signals to reactors when they are triggered
    /// </summary>
    public class PFRoomController : PFBase
    {

        [HideInInspector]
        public List<PFSignalBase> signals = new(); 

        [HideInInspector]
        public List<PFReactorBase> reactors = new();

        [HideInInspector]
        public List<PFActivationHookupEntry> activationHookups = new();

        [HideInInspector]
        public List<PFActivationHookupEntry> deactivationHookups = new();

        public void DispatchSignal(PFSignalBase sender, bool ingress)
        {
            var hookups = ingress ? activationHookups : deactivationHookups;

            foreach (var hookupEntry in hookups)
                if (hookupEntry.signal == sender)
                {
                    foreach (var reactor in hookupEntry.reactors) reactor.React(ingress, sender);
                    break; 
                }
        }
        
        public void RouteSignal(PFSignalBase sender, PFReactorBase reactor, bool ingress)
        {
            reactor.React(ingress, sender);
        }


        /// <summary>
        /// Registers a signal with the room controller
        /// </summary>
        /// <param name="signal"></param>
        public void RegisterSignal(PFSignalBase signal)
        {
            if (signals.Contains(signal))
                return;
            signals.Add(signal);
        }

        /// <summary>
        /// Removes a signal from the room controller
        /// </summary>
        /// <param name="signal"></param>
        public void RemoveSignal(PFSignalBase signal)
        {
            if (signals.Contains(signal))
                signals.Remove(signal);
        }

        public void RegisterReactor(PFReactorBase reactor)
        {
            if (reactors.Contains(reactor))
                return;
            reactors.Add(reactor);
        }

        public void RemoveReactor(PFReactorBase reactor)
        {
            if (reactors.Contains(reactor))
                reactors.Remove(reactor);
        }
    }
}