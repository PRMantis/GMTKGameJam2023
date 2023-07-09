using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    //not really optimal
    [Header("Buttons")]
    [SerializeField] private GameObject PlayButton;
    [SerializeField] private GameObject OptionsButton;
    [SerializeField] private GameObject HowToPlayButton;

    [Header("Menus")]
    [SerializeField] private GameObject OptionsMenu;
    [SerializeField] private GameObject HowToPlayMenu;

    public void OnPressPlay()
    {
        SceneManager.LoadScene(1);
    }

    public void OnPressQuit()
    {
        Application.Quit();
    }

    private void SetButtonState(bool enabled)
    {
        PlayButton.SetActive(enabled);
        OptionsButton.SetActive(enabled);
        HowToPlayButton.SetActive(enabled);
    }

    public void OnPressHowToPlay()
    {
        SetButtonState(false);
        HowToPlayMenu.SetActive(true);
    }

    public void OnPressHowToPlayBack()
    {
        SetButtonState(true);
        HowToPlayMenu.SetActive(false);
    }

    public void OnPressOptions()
    {
        SetButtonState(false);
        OptionsMenu.SetActive(true);
    }

    public void OnPressOptionsBack()
    {
        SetButtonState(true);
        OptionsMenu.SetActive(false);
    }
}
