using System.Collections.Generic;
using UnityEngine.Serialization;

namespace PuzzleForge
{
    public class PuzzleForgeRoomController : PuzzleForgeBase
    {
        public List<PuzzleForgeSignalBase> signals = new List<PuzzleForgeSignalBase>();
        public List<PuzzleForgeReactorBase> reactors = new List<PuzzleForgeReactorBase>();


        public List<ActivationHookupEntry> activationHookups =
            new List<ActivationHookupEntry>();
        
        public List<ActivationHookupEntry> deactivationHookups =
            new List<ActivationHookupEntry>();
        
        public void SendSignal(PuzzleForgeSignalBase sender, bool ingress)
        {
            var hookups = ingress ? activationHookups : deactivationHookups;
    
            foreach (var hookupEntry in hookups)
            {
                if (hookupEntry.signal == sender)
                {
                    foreach (var reactor in hookupEntry.reactors)
                    {
                        reactor.React(ingress);
                    }
                    break; // Assuming only one entry per sender, we break after finding and processing it
                }
            }
        }


        public void RegisterSignal(PuzzleForgeSignalBase signal)
        {
            if (signals.Contains(signal))
                return;
            signals.Add(signal);
        }

        public void RemoveSignal(PuzzleForgeSignalBase signal)
        {
            if (signals.Contains(signal))
                signals.Remove(signal);
        }
        
        public void RegisterReactor(PuzzleForgeReactorBase reactor)
        {
            if (reactors.Contains(reactor))
                return;
            reactors.Add(reactor);
        }

        public void RemoveReactor(PuzzleForgeReactorBase reactor)
        {
            if (reactors.Contains(reactor))
                reactors.Remove(reactor);
        }
    }
}