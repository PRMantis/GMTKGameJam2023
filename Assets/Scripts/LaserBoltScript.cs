using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBoltScript : MonoBehaviour
{
    private BoxCollider2D laserCollider;
    [SerializeField] public int laserDamange = 5;
    [SerializeField] public float boltSpeed = 10f;

    public GameObject target;

    private Vector2 lastPosition; //so that it only goes in straight line
    private Rigidbody2D laserboltRb;

    // Start is called before the first frame update
    void Start()
    {
        laserCollider = GetComponent<BoxCollider2D>();

        Destroy(gameObject, 5f); // Destroys itself after five seconds

        lastPosition = target.transform.position;
        lastPosition.Normalize();

        transform.LookAt(lastPosition);
        laserboltRb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        GoToTarget();
        KeepAxisConsistent();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(!collision.isTrigger)
        {
            if (collision.gameObject.TryGetComponent(out Health health))
            {
                health.TakeDamage(laserDamange);
            }
            Destroy(gameObject);
        }
    }

    public void GoToTarget()
    {
        laserboltRb.AddForce(lastPosition - (Vector2)transform.position);
    }

    private void KeepAxisConsistent()
    {
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, 0, transform.eulerAngles.z);
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
    }
}
