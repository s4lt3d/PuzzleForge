namespace PuzzleForge
{
    interface IPuzzleForgeActivatable
    {
        void Activate(int incomingMask);
        void Deactivate(int incomingMask);
    }
}

