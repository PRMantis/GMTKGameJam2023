using System;
using UnityEngine;

public enum GameState
{
    None,
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

    public void GameEnd(bool hasWon)
    {
        isGameWon = hasWon;
        SetGameState(GameState.GameEnd);
        Debug.Log($"game ends, did win: {hasWon}");
        //game ends
    }
}
