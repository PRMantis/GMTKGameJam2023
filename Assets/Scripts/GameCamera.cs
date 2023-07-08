using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCamera : MonoBehaviour
{
    public static GameCamera instance;

    [SerializeField] private CinemachineVirtualCamera vCam;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        //SetFollowTarget();
    }

    public void SetFollowTarget(Transform target)
    {
        vCam.Follow = target;
    }
}
