using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public PolygonCollider2D enemyHitbox;
    public CircleCollider2D enemyAttackRange;
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

    [SerializeField] GameObject attackTarget;
    [SerializeField] float maxDistanceUntilStopChase = 200f;

    private EnemyStates enemyState;
    // Start is called before the first frame update
    void Start()
    {
        enemyState = EnemyStates.IsMoving;
    }

    // Update is called once per frame
    void Update()
    {
        CalculateTimes();

        if(enemyState == EnemyStates.IsMoving || enemyState == EnemyStates.IsAttackingPlayer)
        {
            EnemyMovement();
        }
        if (enemyState == EnemyStates.IsAttackingAsteroid || enemyState == EnemyStates.IsAttackingPlayer)
        {
            Attack(attackTarget);
        }
        if (Vector2.Distance(transform.position, attackTarget.transform.position) >= maxDistanceUntilStopChase && enemyState == EnemyStates.IsChasingPlayer)
        {
            ChangeState(EnemyStates.IsMoving);
            return;
        }
    }

    private void Attack(GameObject target)
    {
        RotateTowardsPoint(target.transform.position);
        if (timeSinceLastShot >= timeTillShots)
        {
            timeSinceLastShot = 0f;
            var laserBoltNew = Instantiate(laserBolt, gunBarrel.transform.position, laserBolt.transform.rotation);
            laserBoltNew.GetComponent<LaserBoltScript>().target = target;
        }

    }


    //Colliders
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Asteroid")
        {
            if (enemyState == EnemyStates.IsAttackingAsteroid)
            {
                var direction = collision.gameObject.transform.position;
                direction.Normalize();
                enemyRb.velocity = Vector2.zero;

                Attack(collision.gameObject);
            }

        }
        if(enemyState == EnemyStates.IsMoving || enemyState == EnemyStates.IsAttackingAsteroid || enemyState == EnemyStates.IsChasingPlayer)
        {
            if (collision.gameObject.tag == "Player")
            {
                ChangeState(EnemyStates.IsAttackingPlayer);
                attackTarget = collision.gameObject;
            }
        }
        else if(enemyState == EnemyStates.IsMoving && collision.gameObject.tag == "Asteroid")
        {
            ChangeState(EnemyStates.IsAttackingAsteroid);
            attackTarget = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (enemyState == EnemyStates.IsAttackingAsteroid)
        {
            ChangeState(EnemyStates.IsMoving);
        }
        if (enemyState == EnemyStates.IsAttackingPlayer)
        {
            ChangeState(EnemyStates.IsChasingPlayer);
        }
    }

    private void RotateTowardsPoint(Vector2 movementDirection)
    {
        Vector2 difference = movementDirection - (Vector2)transform.position;
        float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotationZ);
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
        if((timeTillLastMovement >= rateOfMovement) || overrideOnCollision || enemyState != EnemyStates.IsAttackingPlayer)
        {
            if(enemyState == EnemyStates.IsMoving)
            {
                var randomSpot = GetRandomSpotInBounds();
                RotateTowardsPoint(randomSpot);
                timeTillLastMovement = 0f;
                enemyRb.velocity = Vector3.zero;

                enemyRb.AddForce(randomSpot, ForceMode2D.Impulse);
            }

        }
        else if (enemyState == EnemyStates.IsAttackingPlayer)
        {
            var playerLocation = attackTarget.transform.position;
            enemyRb.velocity = Vector3.zero;

            enemyRb.AddForce(playerLocation, ForceMode2D.Impulse);
        }

    }

    private Vector2 GetRandomSpotInBounds()
    {
        var movementDirection = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)).normalized;
        var movementPerSecond = movementDirection * enemySpeedForce;

        return movementPerSecond;
    }

    public void StopMovement()
    {
        enemyRb.velocity = Vector3.zero;
        EnemyMovement(true);
    }

    private void CalculateTimes()
    {
        timeSinceLastShot += Time.deltaTime;
        timeTillLastMovement += Time.deltaTime;
    }

    public void ChangeState(EnemyStates newEnemyState)
    {
        enemyState = newEnemyState;
    }
}
