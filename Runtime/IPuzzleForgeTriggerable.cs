namespace PuzzleForge
{
    interface IPuzzleForgeTriggerable
    {
        void Activate(ulong incomingMask);
        void Deactivate(ulong incomingMask);
    }
}

