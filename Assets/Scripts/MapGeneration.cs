using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGeneration : MonoBehaviour
{
    public static MapGeneration Instance;

    public BoxCollider2D mapGenerationMap;
    private Bounds mapGenerationMapBounds;

    public GameObject asteroid;
    public GameObject enemies;
    public GameObject destination;
    public GameObject player;

    public int numberOfAsteroids = 300;

    public int numberOfEnemies = 30;

    public float radiusCheck = 5f;
    // Start is called before the first frame update
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        mapGenerationMapBounds = mapGenerationMap.bounds;
        GenerateMap();
    }


    public void GenerateMap()
    {
        Vector2 GetRandomPoint()
        {
            var minX = mapGenerationMapBounds.min.x;
            var maxX = mapGenerationMapBounds.max.x;

            var minY = mapGenerationMapBounds.min.y;
            var maxY = mapGenerationMapBounds.max.y;

            var randomX = Random.Range(minX, maxX);
            var randomY = Random.Range(minY, maxY);

            return new Vector2(randomX, randomY);
        }

        GameObject TryCreateObject(GameObject obj, bool changeLocation = false)
        {
            var randomPoint = GetRandomPoint();
            var check = Physics2D.OverlapCircle(randomPoint, radiusCheck, LayerMask.GetMask("Default"));

            if (check != null)
            {
                return TryCreateObject(obj);
            }
            else
            {
                if (changeLocation)
                {
                    obj.transform.position = randomPoint;
                    return null;
                }
                else
                {
                    return Instantiate(obj, randomPoint, obj.transform.rotation);
                }
            }
        }

        //First generate asteroid
        for (int i = 0; i < numberOfAsteroids; i++)
        {
            TryCreateObject(asteroid);
        }

        for (int i = 0; i < numberOfEnemies; i++)
        {
            TryCreateObject(enemies);
        }

        AddPlayerAndDestination();

        void AddPlayerAndDestination()
        {
            GameObject playerObject = TryCreateObject(player, true);
            TryCreateObject(destination);

            if (playerObject != null)
            {
                GameManager.Instance.SetPlayer(playerObject.GetComponent<Player>());
            }
        }
    }

}
