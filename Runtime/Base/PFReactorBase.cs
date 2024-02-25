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
    public abstract class PFReactorBase : PFBase
    {
        public ReactorType reactorType;
        public ReactorMode reactorMode;

        public abstract void React(bool ingressState, PFSignalEventBase activator);
        protected abstract void HandleState(bool state);
    }
}