using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public Action OnDie;
    public Action OnTakeDamage;

    [SerializeField] private int maxHealth = 100;

    private int curHealth;

    private void Awake()
    {
        curHealth = maxHealth;
    }

    public void SetHealth(int health)
    {
        maxHealth = health;
        curHealth = health;
    }

    public void TakeDamage(int damage)
    {
        curHealth -= damage;
        OnTakeDamage?.Invoke();

        DynamicWorldText.Instance.ShowActionText(transform.position + Vector3.back, Color.red, $"-{damage}", 0.4f);

        if (curHealth <= 0)
        {
            OnDie?.Invoke();
        }
    }
}
