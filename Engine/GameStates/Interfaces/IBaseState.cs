namespace Engine.GameStates.Interfaces
{
    public interface IGameState
    {
        bool IsGameOver();

        bool IsPaused();
    }
}