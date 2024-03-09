namespace PuzzleForge
{
    public enum ReactorType
    {
        Simple,
        Latching,
        Toggle,
        Combination // not yet used
    }

    public enum ReactorMode
    {
        Normal,
        Inverted
    }

    /// <summary>
    /// The base class for all puzzle forge reactors.
    /// </summary>
    public class PFReactorBase : PFBase
    {

        public ReactorMode reactorMode;

        public virtual void React(bool ingressState, PFSignalEventBase activator)
        {
            // empty
        }

        protected virtual void HandleState(bool state)
        {
            // empty
        }
    }
}