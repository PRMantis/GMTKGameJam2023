using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public PolygonCollider2D enemyHitbox;
    public BoxCollider2D enemyAttackRange;
    public Rigidbody2D enemyRb;

    [SerializeField] public float enemySpeedForce = 50f;

    public GameObject laserBolt;
    [SerializeField] public float minimumCollisionSpeed = 51f;

    public GameObject gunBarrel;
    public BoxCollider2D flightZone;

    private float timeSinceLastShot = 2f;
    private float timeTillShots = 2f;

    private float timeTillLastMovement = 3f;
    private float rateOfMovement = 3f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceLastShot += Time.deltaTime;
        timeTillLastMovement += Time.deltaTime;

        EnemyMovement();
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

    private void EnemyMovement()
    {
        if(timeTillLastMovement >= rateOfMovement)
        {
            var randomSpot = GetRandomSpotInBounds();

            randomSpot.Normalize();
            enemyRb.AddForce(randomSpot * enemySpeedForce, ForceMode2D.Force);
        }

    }

    private Vector2 GetRandomSpotInBounds()
    {
        var minY = flightZone.bounds.min.y;
        var maxY = flightZone.bounds.max.y;

        var minX = flightZone.bounds.min.x;
        var maxX = flightZone.bounds.max.x;

        var randomX = Random.Range(minX, maxX);
        var randomY = Random.Range(minY, maxY);

        return new Vector2(randomX, randomY);
    }
}
