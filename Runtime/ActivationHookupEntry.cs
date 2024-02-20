using System;
using System.Collections.Generic;

namespace PuzzleForge
{
    [Serializable]
    public class ActivationHookupEntry
    {
        public PuzzleForgeSignalBase signal;
        public List<PuzzleForgeReactorBase> reactors = new();
    }
}