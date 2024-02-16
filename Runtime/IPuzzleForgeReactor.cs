namespace PuzzleForge
{
    interface IPuzzleForgeReactor
    {
        void Activate(ulong incomingMask);
        void Deactivate(ulong incomingMask);
    }
}

