using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestinationPlanet : MonoBehaviour
{
    [SerializeField] private ParticleSystem explosionPrefab;
    [SerializeField] private SpriteRenderer mainPlanetRend;


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
            Destroy(Instantiate(explosionPrefab, new Vector3(hitPoint.x, hitPoint.y, -8), Quaternion.identity), 20);
        }

        mainPlanetRend.color = Color.red;

        GameManager.Instance.GameEnd(true);
    }
}
