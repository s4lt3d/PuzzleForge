using System;
using System.Collections.Generic;

namespace PuzzleForge
{
    [Serializable]
    public class PFActivationHookupEntry
    {
        public PFSignalBase signal;
        public List<PFReactorBase> reactors = new();
    }
}