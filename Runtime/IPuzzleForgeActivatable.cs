namespace PuzzleForge
{
    interface IPuzzleForgeActivatable
    {
        void Activate(ulong incomingMask);
        void Deactivate(ulong incomingMask);
    }
}

