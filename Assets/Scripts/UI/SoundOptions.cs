using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundOptions : MonoBehaviour
{
    [SerializeField] private Slider mainSoundSlider;
    [SerializeField] private Slider SFXSlider;
    [SerializeField] private Slider musicSlider;

    private void Awake()
    {
        mainSoundSlider.value = PlayerPrefs.GetFloat(SoundManager.MainLevelSavedDataString, 1);
        SFXSlider.value = PlayerPrefs.GetFloat(SoundManager.SFXLevelSavedDataString, 1);
        musicSlider.value = PlayerPrefs.GetFloat(SoundManager.MusicLevelSavedDataString, 1);

        mainSoundSlider.onValueChanged.AddListener(OnMainSoundSliderChange);
        SFXSlider.onValueChanged.AddListener(OnSFXSliderChange);
        musicSlider.onValueChanged.AddListener(OnMusicSliderChange);
    }

    private void OnDestroy()
    {
        mainSoundSlider.onValueChanged.RemoveAllListeners();
        SFXSlider.onValueChanged.RemoveAllListeners();
        musicSlider.onValueChanged.RemoveAllListeners();
    }

    void Start()
    {

    }

    private void OnMainSoundSliderChange(float value)
    {
        SoundManager.Instance.SetMainSoundVolume(value);
    }

    private void OnSFXSliderChange(float value)
    {
        SoundManager.Instance.SetSFXVolume(value);
    }

    private void OnMusicSliderChange(float value)
    {
        SoundManager.Instance.SetMusicVolume(value);
    }
}
