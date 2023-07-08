using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private GameCamera gameCamera;
    [SerializeField] private Player player;//redo into spawned from script player

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

        gameCamera.SetFollowTarget(player.transform);
    }

    void Start()
    {

    }


}
