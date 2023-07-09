using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Health))]
public class Asteroid : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float movePower = 20;
    [SerializeField] private float moveBoostPower = 40;
    [SerializeField] private float pushPowerMultiplier = 10;
    [SerializeField] private float takeDamageSpeedThreshold = 4.5f;

    [Header("References")]
    [SerializeField] private ParticleSystem hitPrefab;
    [SerializeField] private ParticleSystem explosionPrefab;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Health health;

    //[Header("Sounds")]
    //[SerializeField] private AudioClip breakSound;

    private bool isPlayer = false;

    private void Awake()
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody2D>();
        }

        if (health == null)
        {
            health = GetComponent<Health>();
        }

        health.OnDie += OnDie;
        health.OnTakeDamage += TakeDamage;

        isPlayer = GetComponent<Player>() != null;
    }

    private void OnDestroy()
    {
        health.OnDie -= OnDie;
        health.OnTakeDamage -= TakeDamage;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        float collisionMagnitude = collision.relativeVelocity.magnitude;
        Health health = collision.transform.GetComponent<Health>();

        //asteroid pushing
        if (collision.gameObject.GetComponent<Player>() != null)//player doesn't take damage
        {
            ContactPoint2D contactPoint = collision.GetContact(0);
            float power = collisionMagnitude * pushPowerMultiplier;

            rb.AddForce(power * contactPoint.normal, ForceMode2D.Impulse);
            contactPoint.rigidbody.velocity = Vector2.zero;
        }
        else if (health != null && collisionMagnitude > takeDamageSpeedThreshold)//damage of asteroids
        {
            int damage = Mathf.RoundToInt(collisionMagnitude / 2) * 2;
            health.TakeDamage(damage);
            Debug.Log($"{collision.gameObject.name} takes damage: {damage}");
        }

        DestinationPlanet destinationPlanet = collision.transform.GetComponentInParent<DestinationPlanet>();
        if (destinationPlanet != null && isPlayer)
        {
            destinationPlanet.OnPlayerHitsPlanet(collision.GetContact(0).point);
            this.health.TakeDamage(this.health.GetHealth());//destroy player
        }
    }

    private void OnDie()
    {
        if (isPlayer && GameManager.Instance.GetGameState() != GameState.GameEnd)
        {
            GameManager.Instance.GameEnd(false);
        }

        if (explosionPrefab != null)
        {
            Destroy(Instantiate(explosionPrefab, transform.position, Quaternion.identity), 2);
        }

        Destroy(gameObject);
    }

    private void TakeDamage(int damage)
    {
        if (hitPrefab != null)
        {
            Destroy(Instantiate(hitPrefab, transform.position, Quaternion.identity), 2);
        }
    }

    public void ApplyForce(Vector2 direction)
    {
        rb.AddForce(direction * movePower, ForceMode2D.Force);
    }

    public void ApplyBoostForce(Vector2 direction)
    {
        rb.AddForce(direction * moveBoostPower, ForceMode2D.Impulse);
    }
}
