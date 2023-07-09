using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour
{
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject gameWonPanel;
    [SerializeField] private GameObject pausedPanel;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text gameWonscoreText;

    void Start()
    {
        GameManager.Instance.OnGameStateChange += OnGameStateChange;
        GameManager.Instance.OnScoreChange += OnScoreChanged;
        gameOverPanel.SetActive(false);
        gameWonPanel.SetActive(false);
        pausedPanel.SetActive(false);
        scoreText.gameObject.SetActive(true);
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnGameStateChange -= OnGameStateChange;
        GameManager.Instance.OnScoreChange -= OnScoreChanged;
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
                scoreText.gameObject.SetActive(false);
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

    private void OnScoreChanged(int score)
    {
        scoreText.text = $"Score :{score}";
        gameWonscoreText.text = $"Score :{score}";
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
