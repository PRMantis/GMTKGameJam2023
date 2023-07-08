using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]
[RequireComponent(typeof(PlayerInput))]
public class Player : MonoBehaviour
{
    public Health Health { get; private set; }

    private PlayerInput input;

    private void Awake()
    {
        if (Health == null)
        {
            Health = GetComponent<Health>();
        }

        if (input == null)
        {
            input = GetComponent<PlayerInput>();
        }

        Health.OnDie += OnPlayerDie;
    }

    void Start()
    {

    }

    private void OnPlayerDie()
    {
        input.SetInputState(false);//disable input 
    }
}
