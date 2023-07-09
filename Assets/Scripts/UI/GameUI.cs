using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour
{
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject gameWonPanel;
    [SerializeField] private GameObject pausedPanel;

    void Start()
    {
        GameManager.Instance.OnGameStateChange += OnGameStateChange;
        gameOverPanel.SetActive(false);
        gameWonPanel.SetActive(false);
        pausedPanel.SetActive(false);
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnGameStateChange -= OnGameStateChange;
    }

    private void OnGameStateChange(GameState state)
    {
        switch (state)
        {
            case GameState.Paused:
                pausedPanel.SetActive(true);
                break;
            case GameState.Running:
                pausedPanel.SetActive(false);
                break;
            case GameState.GameEnd:
                pausedPanel.SetActive(false);
                if (GameManager.Instance.IsGameWon())
                {
                    gameWonPanel.SetActive(true);
                }
                else
                {
                    gameOverPanel.SetActive(true);
                }
                break;
        }
    }

    public void OnPressRestart()
    {
        SceneManager.LoadScene(1);
    }

    public void OnPressExit()
    {
        SceneManager.LoadScene(0);
    }

    public void OnPressBack()
    {
        GameManager.Instance.UnPauseGame();
    }
}
