using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Health : MonoBehaviour
{
    public Action OnDie;
    public Action<int> OnTakeDamage;

    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int curHealth;
    [SerializeField] private float hitVolume = 1f;

    [Header("Sounds")]
    [SerializeField] private AudioClip[] takehitSounds;
    [SerializeField] private AudioClip[] dieSounds;

    private void Awake()
    {
        curHealth = maxHealth;
    }

    public void SetHealth(int health)
    {
        maxHealth = health;
        curHealth = health;
    }

    public int GetMaxHealth()
    {
        return maxHealth;
    }

    public int GetHealth()
    {
        return curHealth;
    }

    public void TakeDamage(int damage)
    {
        curHealth -= damage;
        OnTakeDamage?.Invoke(damage);

        DynamicWorldText.Instance.ShowActionText(transform.position + Vector3.back, Color.red, $"-{damage}", 0.4f);

        if (curHealth <= 0)
        {
            SoundManager.Instance.PlaySound(dieSounds, transform.position, SoundManager.Instance.GetAudioMixerGroup(AudioGroup.SFX));
            OnDie?.Invoke();
        }
        else
        {
            SoundManager.Instance.PlaySound(takehitSounds, transform.position, SoundManager.Instance.GetAudioMixerGroup(AudioGroup.SFX), volume: hitVolume);
        }
    }
}
