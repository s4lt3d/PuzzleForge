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

    public abstract class PFReactorBase : PFBase
    {
        public ReactorType reactorType;
        public ReactorMode reactorMode;

        public abstract void React(bool ingressState);
    }
}