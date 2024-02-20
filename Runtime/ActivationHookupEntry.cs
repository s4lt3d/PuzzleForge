using System.Collections.Generic;

namespace PuzzleForge
{
    [System.Serializable]
    public class ActivationHookupEntry
    {
        public PuzzleForgeSignalBase signal;
        public List<PuzzleForgeReactorBase> reactors = new List<PuzzleForgeReactorBase>();
    }
}