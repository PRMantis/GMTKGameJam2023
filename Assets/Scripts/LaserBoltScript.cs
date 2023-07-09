using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBoltScript : MonoBehaviour
{
    private BoxCollider2D laserCollider;
    [SerializeField] public int laserDamange = 5;

    [SerializeField] public float boltForce = 100f;

    [Header("Sounds")]
    [SerializeField] public AudioClip[] shootSound;

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

        transform.localRotation = rotation;
        GoToTarget();

        SoundManager.Instance.PlaySound(shootSound, transform.position, SoundManager.Instance.GetAudioMixerGroup(AudioGroup.SFX));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.isTrigger)
        {
            if (collision.gameObject.TryGetComponent(out Health health))
            {
                health.TakeDamage(laserDamange);
                Destroy(gameObject);
            }

            if (collision.transform.parent != null && collision.transform.parent.TryGetComponent(out Health health2))
            {
                health2.TakeDamage(laserDamange);
                Destroy(gameObject);
            }
        }
    }

    public void GoToTarget()
    {
        laserboltRb.velocity = Vector2.zero; //so that it stops moving
        laserboltRb.angularVelocity = 0; //so that it stops moving
        laserboltRb.constraints = RigidbodyConstraints2D.FreezeRotation;

        laserboltRb.AddForce((lastPosition - (Vector2)transform.position).normalized * boltForce, ForceMode2D.Impulse);
    }
}
