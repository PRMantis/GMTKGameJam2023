using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSoundtrack : MonoBehaviour
{
    [SerializeField] private AudioClip soundtrack;

    void Start()
    {
        //SoundManager.Instance.PlaySound(soundtrack, Camera.main.transform,
        //    SoundManager.Instance.GetAudioMixerGroup(AudioGroup.Music), isLooping: true, volume: 0.5f);
    }
}
