using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBoltScript : MonoBehaviour
{
    private BoxCollider2D laserCollider;
    [SerializeField] public int laserDamange = 5;

    [SerializeField] public float boltForce = 100f;

    public GameObject target;

    private Vector2 lastPosition; //so that it only goes in straight line
    private Rigidbody2D laserboltRb;
    public Quaternion rotation;

    // Start is called before the first frame update
    void Start()
    {
        laserCollider = GetComponent<BoxCollider2D>();

        Destroy(gameObject, 5f); // Destroys itself after five seconds

        lastPosition = target.transform.position;

        transform.LookAt(lastPosition);
        laserboltRb = GetComponent<Rigidbody2D>();

        GoToTarget();
    }

    private void Update()
    {
        KeepAxisConsistent();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!collision.isTrigger)
        {
            if (collision.gameObject.TryGetComponent(out Health health))
            {
                health.TakeDamage(laserDamange);
                Destroy(gameObject);
            }
        }
    }

    public void GoToTarget()
    {
        laserboltRb.velocity = Vector2.zero; //so that it stops moving
        laserboltRb.angularVelocity = 0; //so that it stops moving

        laserboltRb.AddForce((lastPosition - (Vector2)transform.position).normalized * boltForce, ForceMode2D.Impulse);
    }

    private void RotateTowardsEnemy(Vector2 direction)
    {
        Quaternion toRotation = Quaternion.LookRotation(Vector2.up, direction);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, 50f * Time.deltaTime);
    }

    private void KeepAxisConsistent()
    {
        transform.localRotation = rotation;
        //transform.position = new Vector3(transform.position.x, transform.position.y, 0);
    }
}
