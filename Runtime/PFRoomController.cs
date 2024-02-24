using System.Collections.Generic;
using UnityEngine;

namespace PuzzleForge
{
    /// <summary>
    /// This class is a controller for a room, responsible for managing signals and reactors grouped within a room.
    /// It sends signals to reactors when they are triggered.
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

        /// <summary>
        /// Dispatches a signal to the appropriate reactors based on the sender and ingress value.
        /// </summary>
        /// <param name="sender">The signal that triggered the dispatch.</param>
        /// <param name="ingress">True if the signal is an ingress signal, false if it is a egress signal.</param>
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

        /// <summary>
        /// Routes a signal to a reactor in the room controller.
        /// </summary>
        /// <param name="sender">The signal sending the message.</param>
        /// <param name="reactor">The reactor to receive the signal.</param>
        /// <param name="ingress">Indicates if the signal is an ingress signal.</param>
        public void RouteSignal(PFSignalBase sender, PFReactorBase reactor, bool ingress)
        {
            reactor.React(ingress, sender);
        }


        /// <summary>
        /// Registers a signal in the room controller's list of registered signals.
        /// </summary>
        /// <param name="signal">The signal to register.</param>
        public void RegisterSignal(PFSignalBase signal)
        {
            if (signals.Contains(signal))
                return;
            signals.Add(signal);
        }


        /// <summary>
        /// Removes a signal from the room controller's list of registered signals.
        /// </summary>
        /// <param name="signal">The signal to be removed.</param>
        public void RemoveSignal(PFSignalBase signal)
        {
            if (signals.Contains(signal))
                signals.Remove(signal);
        }

        /// <summary>
        /// Registers a reactor with the room controller.
        /// If the reactor is already registered, the method simply returns.
        /// Otherwise, the reactor is added to the list of reactors.
        /// </summary>
        /// <param name="reactor">The reactor to register.</param>
        public void RegisterReactor(PFReactorBase reactor)
        {
            if (reactors.Contains(reactor))
                return;
            reactors.Add(reactor);
        }

        /// <summary>
        /// Removes a reactor from the room controller.
        /// If the reactor is already removed or is not registered, the method simply returns.
        /// </summary>
        /// <param name="reactor">The reactor to remove.</param>
        public void RemoveReactor(PFReactorBase reactor)
        {
            if (reactors.Contains(reactor))
                reactors.Remove(reactor);
        }
    }
}