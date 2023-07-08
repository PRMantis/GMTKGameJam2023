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

        if(enemyState == EnemyStates.IsMoving || enemyState == EnemyStates.IsChasingPlayer)
        {
            EnemyMovement();
        }
        if (enemyState == EnemyStates.IsAttackingAsteroid || enemyState == EnemyStates.IsAttackingPlayer)
        {
            Attack(attackTarget);
        }
        if(enemyState == EnemyStates.IsChasingPlayer)
        {
            if (Vector2.Distance(transform.position, attackTarget.transform.position) >= maxDistanceUntilStopChase)
            {
                ChangeState(EnemyStates.IsMoving);
                return;
            }
        }
    }

    private void Attack(GameObject target)
    {
        if(target == null)
        {
            ChangeState(EnemyStates.IsMoving);
        }
        var rotation = RotateTowardsPoint(target.transform.position);
        if (timeSinceLastShot >= timeTillShots)
        {
            timeSinceLastShot = 0f;
            var laserBoltNew = Instantiate(laserBolt, gunBarrel.transform.position, rotation);
            laserBoltNew.GetComponent<LaserBoltScript>().target = target;
            laserBoltNew.GetComponent<LaserBoltScript>().rotation = rotation;
        }

    }


    //Colliders
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (enemyState == EnemyStates.IsMoving && collision.gameObject.tag == "Asteroid")
        {
            ChangeState(EnemyStates.IsAttackingAsteroid);
            attackTarget = collision.gameObject;
        }
        else if (enemyState == EnemyStates.IsMoving || enemyState == EnemyStates.IsAttackingAsteroid || enemyState == EnemyStates.IsChasingPlayer)
        {
            if (collision.gameObject.tag == "Player")
            {
                ChangeState(EnemyStates.IsAttackingPlayer);
                attackTarget = collision.gameObject;
            }
        }

    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Asteroid")
        {
            if (enemyState == EnemyStates.IsAttackingAsteroid)
            {
                ChangeState(EnemyStates.IsMoving);
            }
        }

        if (collision.gameObject.tag == "Player")
        {
            if (enemyState == EnemyStates.IsAttackingPlayer)
            {
                ChangeState(EnemyStates.IsChasingPlayer);
            }
        }


    }

    private Quaternion RotateTowardsPoint(Vector2 movementDirection)
    {
        Vector2 difference = movementDirection - (Vector2)transform.position;
        float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        var offset = -90f;
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotationZ + offset);
        return transform.rotation;
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
        if (enemyState == EnemyStates.IsChasingPlayer)
        {
            var playerLocation = attackTarget.transform.position;
            RotateTowardsPoint(playerLocation);
            timeTillLastMovement = 0f;
            enemyRb.velocity = Vector3.zero;

            enemyRb.AddForce((playerLocation - transform.position).normalized * enemySpeedForce, ForceMode2D.Impulse);
        }
        else if ((timeTillLastMovement >= rateOfMovement) || overrideOnCollision)
        {
            if(enemyState == EnemyStates.IsMoving)
            {
                var randomSpot = GetRandomSpotInBounds();
                RotateTowardsPoint(randomSpot);
                timeTillLastMovement = 0f;
                enemyRb.velocity = Vector3.zero;

                enemyRb.AddForce((randomSpot - (Vector2)transform.position).normalized * enemySpeedForce, ForceMode2D.Impulse);
            }

        }


    }

    private Vector2 GetRandomSpotInBounds()
    {
        var movementDirection = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));
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
