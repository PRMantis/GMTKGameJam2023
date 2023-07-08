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

    [SerializeField] private ParticleSystem asteroidBreakParticles;
    [SerializeField] private List<GameObject> healthIndicators;
    [SerializeField] private float boostRechargeTime = 2;//in seconds

    [Header("Sounds")]
    [SerializeField] private AudioClip breakSound;//in seconds

    private float boostPower;
    private PlayerInput input;
    private Asteroid asteroid;
    private int healthPartsCount;

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

        healthPartsCount = Health.GetMaxHealth() / healthIndicators.Count;

        boostPower = 0;
        Health.OnDie += OnPlayerDie;
        Health.OnTakeDamage += OnPlayerTakeDamage;
    }

    private void OnDestroy()
    {
        Health.OnDie -= OnPlayerDie;
        Health.OnTakeDamage -= OnPlayerTakeDamage;
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

    private void OnPlayerTakeDamage(int damage)
    {
        int curHealth = Health.GetHealth();
        for (int i = 0; i < healthIndicators.Count; i++)
        {
            if (curHealth < (i + 1) * healthPartsCount)
            {
                if (healthIndicators[i].activeSelf)
                {
                    if (asteroidBreakParticles != null)
                    {
                        Destroy(Instantiate(asteroidBreakParticles, transform.position, Quaternion.identity), 2);
                    }

                    healthIndicators[i].SetActive(false);

                    SoundManager.Instance.PlaySound(breakSound, transform.position, SoundManager.Instance.GetAudioMixerGroup(AudioGroup.SFX));
                }
            }
        }
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
