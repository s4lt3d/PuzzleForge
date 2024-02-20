using System.Collections.Generic;
using UnityEngine;

namespace PuzzleForge
{
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

        public void SendSignal(PFSignalBase sender, bool ingress)
        {
            var hookups = ingress ? activationHookups : deactivationHookups;

            foreach (var hookupEntry in hookups)
                if (hookupEntry.signal == sender)
                {
                    foreach (var reactor in hookupEntry.reactors) reactor.React(ingress);
                    break; // Assuming only one entry per sender, we break after finding and processing it
                }
        }


        public void RegisterSignal(PFSignalBase signal)
        {
            if (signals.Contains(signal))
                return;
            signals.Add(signal);
        }

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