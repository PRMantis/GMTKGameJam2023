using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGeneration : MonoBehaviour
{
    public static MapGeneration Instance;

    public BoxCollider2D mapGenerationMap;
    private Bounds mapGenerationMapBounds;

    [SerializeField] public GameObject largeAsteroid;
    [SerializeField] public GameObject mediumAsteroid;
    [SerializeField] public GameObject smallAsteroid;

    [SerializeField] public GameObject enemies;
    [SerializeField] public GameObject destination;
    [SerializeField] public GameObject player;

    [SerializeField] public int numberOfSmallAsteroids = 300;
    [SerializeField] public int numberOfMediumAsteroids = 300;
    [SerializeField] public int numberOfLargeAsteroids = 300;

    [SerializeField] public BoxCollider2D playerSpawnTop;
    [SerializeField] public BoxCollider2D playerSpawnBottom;

    private int useTopRandom = 0;

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

        //First generate asteroids
        for (int i = 0; i < numberOfLargeAsteroids; i++)
        {
            TryCreateObject(largeAsteroid);
        }


        for (int i = 0; i < numberOfMediumAsteroids; i++)
        {
            TryCreateObject(mediumAsteroid);
        }


        for (int i = 0; i < numberOfSmallAsteroids; i++)
        {
            TryCreateObject(smallAsteroid);
        }

        for (int i = 0; i < numberOfEnemies; i++)
        {
            TryCreateObject(enemies);
        }

        AddPlayerAndDestination();

        //
        void AddPlayerAndDestination()
        {
            GameObject playerObject = TryCreatePlayer(player, true);
            TryCreateDestination(destination);

            if (playerObject != null)
            {
                GameManager.Instance.SetPlayer(playerObject.GetComponent<Player>());
            }
        }

        Vector2 GetRandomPoint(Bounds? bounds)
        {
            if (bounds.HasValue)
            {
                var minX = bounds.Value.min.x;
                var maxX = bounds.Value.max.x;

                var minY = bounds.Value.min.y;
                var maxY = bounds.Value.max.y;

                var randomX = Random.Range(minX, maxX);
                var randomY = Random.Range(minY, maxY);

                return new Vector2(randomX, randomY);
            }
            else
            {
                var minX = mapGenerationMapBounds.min.x;
                var maxX = mapGenerationMapBounds.max.x;

                var minY = mapGenerationMapBounds.min.y;
                var maxY = mapGenerationMapBounds.max.y;

                var randomX = Random.Range(minX, maxX);
                var randomY = Random.Range(minY, maxY);

                return new Vector2(randomX, randomY);
            }
        }

        GameObject TryCreateObject(GameObject obj, bool changeLocation = false)
        {
            var randomPoint = GetRandomPoint(null);
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

        GameObject TryCreatePlayer(GameObject obj, bool changeLocation = false)
        {
            useTopRandom = Random.Range(1, 2);
            var randomPoint = Vector2.zero;

            if (useTopRandom == 1)
            {
                randomPoint = GetRandomPoint(playerSpawnTop.bounds);
            }
            else
            {
                randomPoint = GetRandomPoint(playerSpawnBottom.bounds);
            }


            var check = Physics2D.OverlapCircle(randomPoint, radiusCheck, LayerMask.GetMask("Default"));

            if (check != null)
            {
                return TryCreatePlayer(player, true);
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

        GameObject TryCreateDestination(GameObject obj, bool changeLocation = false)
        {
            var randomPoint = Vector2.zero;

            if (useTopRandom == 1)
            {
                randomPoint = GetRandomPoint(playerSpawnBottom.bounds);
            }
            else
            {
                randomPoint = GetRandomPoint(playerSpawnTop.bounds);
            }


            var check = Physics2D.OverlapCircle(randomPoint, radiusCheck, LayerMask.GetMask("Default"));

            if (check != null)
            {
                return TryCreateDestination(obj);
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
    }

}
