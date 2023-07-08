using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private GameObject PlayButton;
    [SerializeField] private GameObject OptionsButton;

    [Header("Menus")]
    [SerializeField] private GameObject OptionsMenu;

    public void OnPressPlay()
    {
        SceneManager.LoadScene(1);
    }

    public void OnPressOptions()
    {
        PlayButton.SetActive(false);
        OptionsButton.SetActive(false);
        OptionsMenu.SetActive(true);
    }

    public void OnPressQuit()
    {
        Application.Quit();
    }


    public void OnPressOptionsBack()
    {
        PlayButton.SetActive(true);
        OptionsButton.SetActive(true);
        OptionsMenu.SetActive(false);
    }
}
