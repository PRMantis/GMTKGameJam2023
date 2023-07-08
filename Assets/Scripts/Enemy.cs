using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public PolygonCollider2D enemyHitbox;
    public BoxCollider2D enemyAttackRange;

    [SerializeField] public float enemySpeed = 5f;

    public GameObject laserBolt;
    [SerializeField] public float minimumCollisionSpeed = 51f;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void AttackRandomAsteroid()
    {

    }

    private void Attack()
    {

    }

    //Colliders
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player" || collision.gameObject.tag == "Asteroid")
        {
            Attack();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.TryGetComponent(out Asteroid asteroid))
        {
            if(asteroid.GetSpeed() >= minimumCollisionSpeed)
            {
                Destroy(gameObject);
            }
        }
    }
}
