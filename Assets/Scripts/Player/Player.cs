using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

[RequireComponent(typeof(Health))]
[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(Asteroid))]
public class Player : MonoBehaviour
{
    public Action<float> OnBoostChange;

    public Health Health { get; private set; }

    [SerializeField] private float boostRechargeTime = 2;//in seconds

    private float boostPower;
    private PlayerInput input;
    private Asteroid asteroid;

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

        if (asteroid == null)
        {
            asteroid = GetComponent<Asteroid>();
        }

        boostPower = 0;
        Health.OnDie += OnPlayerDie;
    }

    void Start()
    {

    }

    private void Update()
    {

        if (boostPower >= 1)
        {
            ChangeBoost(1);
        }
        else
        {
            boostPower += Time.deltaTime / boostRechargeTime;
            ChangeBoost(boostPower);
        }
    }

    private void ChangeBoost(float value)
    {
        boostPower = value;
        OnBoostChange?.Invoke(boostPower);
    }

    private void OnPlayerDie()
    {
        input.SetInputState(false);//disable input 
    }

    public void TryBoost(Vector2 direction)
    {
        asteroid.ApplyBoostForce(direction * boostPower);
        ChangeBoost(0);
    }
}
