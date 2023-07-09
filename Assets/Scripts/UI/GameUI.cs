using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour
{
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject gameWonPanel;

    private void Awake()
    {
        GameManager.Instance.OnGameStateChange += OnGameStateChange;
    }


    void Start()
    {
        gameOverPanel.SetActive(false);
        gameWonPanel.SetActive(false);
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnGameStateChange -= OnGameStateChange;
    }

    private void OnGameStateChange(GameState state)
    {
        switch (state)
        {
            case GameState.None:

                break;
            case GameState.Running:

                break;
            case GameState.GameEnd:
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

    public void OnPressBack()
    {
        SceneManager.LoadScene(0);
    }
}
