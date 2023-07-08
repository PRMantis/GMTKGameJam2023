using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBoltScript : MonoBehaviour
{
    private BoxCollider2D laserCollider;
    [SerializeField] public int laserDamange = 5;

    // Start is called before the first frame update
    void Start()
    {
        laserCollider = GetComponent<BoxCollider2D>();

        Destroy(gameObject, 5f); // Destroys itself after five seconds
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Asteroid")
        {
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<Health>().TakeDamage(laserDamange);
            Destroy(collision.gameObject);
        }

        Destroy(gameObject);
    }
}
