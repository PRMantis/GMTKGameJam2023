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

    public GameObject gunBarrel;

    private float timeSinceLastShot = 2f;
    private float timeTillShots = 2f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceLastShot += Time.deltaTime;
    }

    private void AttackRandomAsteroid()
    {

    }

    private void Attack(GameObject target)
    {
        timeSinceLastShot = 0f;
        var laserBoltNew = Instantiate(laserBolt, gunBarrel.transform.position, laserBolt.transform.rotation);
        laserBoltNew.GetComponent<LaserBoltScript>().target = target;
    }

    //Colliders
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player" || collision.gameObject.tag == "Asteroid" && timeSinceLastShot >= timeTillShots)
        {
            var direction = collision.gameObject.transform.position;
            direction.Normalize();

            RotateTowardsEnemy(collision.gameObject.transform.position);
            Attack(collision.gameObject);
        }
    }

    private void RotateTowardsEnemy(Vector2 direction)
    {
        Quaternion toRotation = Quaternion.LookRotation(Vector2.up, direction);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, 50f * Time.deltaTime);
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
