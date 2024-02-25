using System;
using System.Collections.Generic;

namespace PuzzleForge
{
    /// <summary>
    /// Represents an entry for activation hookups in a room controller.
    /// A work around helper class to allow Unity to serialize a dictionary.
    /// </summary>
    [Serializable]
    public class PFActivationHookupEntry
    {
        public PFSignalEventBase signal;
        public List<PFReactorBase> reactors = new();
    }
}