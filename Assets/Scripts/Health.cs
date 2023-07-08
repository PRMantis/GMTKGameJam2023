using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public Action OnDie;
    public Action<int> OnTakeDamage;

    [SerializeField] private int maxHealth = 100;

    [SerializeField] private int curHealth;

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
            OnDie?.Invoke();
        }
    }
}
