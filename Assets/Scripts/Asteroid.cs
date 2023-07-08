using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Asteroid : MonoBehaviour
{
    [SerializeField] private float power = 20;
    [SerializeField] private float hitPowerMultiplier = 10;
    [SerializeField] private Rigidbody2D rb;

    private void Awake()
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody2D>();
        }
    }

    public void ApplyForce(Vector2 direcction)
    {
        rb.AddForce(direcction * power, ForceMode2D.Force);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Player>() != null)
        {
            ContactPoint2D contactPoint = collision.GetContact(0);

            float power = contactPoint.rigidbody.velocity.magnitude * contactPoint.rigidbody.mass;
            rb.AddForce(contactPoint.normal * power * hitPowerMultiplier, ForceMode2D.Impulse);
            contactPoint.rigidbody.velocity = Vector2.zero;
        }
    }

    public float GetSpeed()
    {
        return 50f; // change later
    }
}
