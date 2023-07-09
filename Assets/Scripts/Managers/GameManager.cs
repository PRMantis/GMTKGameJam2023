using System;
using UnityEngine;

public enum GameState
{
    Paused,
    Running,
    GameEnd
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public Action<GameState> OnGameStateChange;

    [SerializeField] private GameCamera gameCamera;
    [SerializeField] private Player player;//redo into spawned from script player

    private GameState gameState;
    private bool isGameWon;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        if (gameCamera != null && player != null)
        {
            gameCamera.SetFollowTarget(player.transform);
        }
    }

    void Start()
    {
        SetGameState(GameState.Running);
    }

    private void SetGameState(GameState state)
    {
        gameState = state;
        OnGameStateChange?.Invoke(state);

        if (state == GameState.Paused)
        {
            Time.timeScale = 0.0f;
        }
        else
        {
            Time.timeScale = 1.0f;
        }
    }

    public GameState GetGameState()
    {
        return gameState;
    }

    public void SetPlayer(Player player)
    {
        this.player = player;
        gameCamera.SetFollowTarget(player.transform);
    }

    public Player GetPlayer()
    {
        return player;
    }

    public bool IsGameWon()
    {
        return isGameWon;
    }

    public void TryPauseGame()
    {
        if (gameState == GameState.Running)
        {
            SetGameState(GameState.Paused);
        }
    }

    public void UnPauseGame()
    {
        if (gameState == GameState.Paused)
        {
            SetGameState(GameState.Running);
        }
    }

    public void GameEnd(bool hasWon)
    {
        isGameWon = hasWon;
        SetGameState(GameState.GameEnd);
        Debug.Log($"game ends, did win: {hasWon}");
        //game ends
    }
}
