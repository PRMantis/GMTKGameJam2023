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
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Health health;

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
    }

    public void ApplyForce(Vector2 direction)
    {
        rb.AddForce(direction * movePower, ForceMode2D.Force);
    }

    public void ApplyBoostForce(Vector2 direction)
    {
        rb.AddForce(direction * moveBoostPower, ForceMode2D.Impulse);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        float collisionMagnitude = collision.relativeVelocity.magnitude;
        Health health = collision.transform.GetComponent<Health>();

        //damage of asteroids
        if (health != null && collisionMagnitude > takeDamageSpeedThreshold)
        {
            int damage = Mathf.RoundToInt(collisionMagnitude / 2) * 2;
            health.TakeDamage(damage);
            //this.health.TakeDamage(damage);
            Debug.Log($"{collision.gameObject.name} takes damage: {damage}");
        }

        //asteroid pushing
        if (collision.gameObject.GetComponent<Player>() != null)
        {
            ContactPoint2D contactPoint = collision.GetContact(0);
            float power = collisionMagnitude * pushPowerMultiplier;

            rb.AddForce(power * contactPoint.normal, ForceMode2D.Impulse);
            contactPoint.rigidbody.velocity = Vector2.zero;
        }
    }

    public float GetSpeed()
    {
        return 50f; // change later
    }
}
