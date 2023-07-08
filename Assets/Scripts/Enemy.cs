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

    [SerializeField] private float timeSinceLastShot = 2f;
    [SerializeField] private float timeTillShots = 2f;

    [SerializeField] private float timeTillLastMovement = 3f;
    [SerializeField] private float rateOfMovement = 5f;

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
        if(timeSinceLastShot >= timeTillShots)
        {
            timeSinceLastShot = 0f;
            var laserBoltNew = Instantiate(laserBolt, gunBarrel.transform.position, laserBolt.transform.rotation);
            laserBoltNew.GetComponent<LaserBoltScript>().target = target;
        }

    }

    //Colliders
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player" || collision.gameObject.tag == "Asteroid")
        {
            var direction = collision.gameObject.transform.position;
            direction.Normalize();

            //RotateTowardsEnemy(collision.gameObject.transform.position);
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

    private void EnemyMovement(bool overrideOnCollision = false)
    {
        if((timeTillLastMovement >= rateOfMovement) || overrideOnCollision)
        {
            timeTillLastMovement = 0f;
            var randomSpot = GetRandomSpotInBounds();
            Debug.Log(randomSpot);

            enemyRb.velocity = Vector3.zero;

            enemyRb.AddForce(randomSpot, ForceMode2D.Impulse);
        }

    }

    private Vector2 GetRandomSpotInBounds()
    {
        var movementDirection = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)).normalized;
        var movementPerSecond = movementDirection * enemySpeedForce;


        Vector2 difference = movementDirection - (Vector2)transform.position;
        float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotationZ);
        return movementPerSecond;
    }

    public void StopMovement()
    {
        enemyRb.velocity = Vector3.zero;
        EnemyMovement(true);
    }
}
