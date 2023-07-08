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

    [SerializeField] private AudioClip takehitSound;

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
        SoundManager.Instance.PlaySound(takehitSound, transform.position, SoundManager.Instance.GetAudioMixerGroup(AudioGroup.SFX));

        if (curHealth <= 0)
        {
            OnDie?.Invoke();
        }
    }
}
