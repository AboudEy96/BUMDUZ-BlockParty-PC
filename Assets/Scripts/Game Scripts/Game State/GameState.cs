using UnityEngine;

public enum GameState
{
    WaitingToStart,
    Playing,
    GameOver
}

public static class GameStateManager
{
    public static GameState CurrentState { get; private set; } = GameState.WaitingToStart;

    public static void SetState(GameState newState) => CurrentState = newState;

    public static bool IsPlaying()        => CurrentState == GameState.Playing;
    public static bool IsWaitingToStart() => CurrentState == GameState.WaitingToStart;
    public static bool IsGameOver()       => CurrentState == GameState.GameOver;
}