using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestinationPlanet : MonoBehaviour
{
    [SerializeField] private ParticleSystem explosionPrefab;


    private void Awake()
    {

    }

    void Start()
    {

    }

    public void OnPlayerHitsPlanet(Vector2 hitPoint)
    {
        //particle explosion
        if (explosionPrefab != null)
        {
            Destroy(Instantiate(explosionPrefab, hitPoint, Quaternion.identity), 2);
        }

        GameManager.Instance.GameEnd(true);
    }
}
