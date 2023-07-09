using Cinemachine;
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

    public void SetFollowTarget(Transform target)
    {
        vCam.Follow = target;
    }
}
