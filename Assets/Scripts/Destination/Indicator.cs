using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Indicator : MonoBehaviour
{
    [SerializeField] private float offScreenThreshold = 10f;
    [SerializeField] private GameObject indicatorPrefab;
    [SerializeField] private Player player;

    private GameObject indicator;
    private Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main;
        indicator = Instantiate(indicatorPrefab);
        indicator.SetActive(false);
    }

    private void Start()
    {
        player = GameManager.Instance.GetPlayer();
    }

    void Update()
    {
        if (player == null)
        {
            player = GameManager.Instance.GetPlayer();
            return;
        }

        Vector3 targetDirection = transform.position - player.transform.position;
        float distanceToTarget = targetDirection.magnitude;

        if (distanceToTarget < offScreenThreshold)
        {
            indicator.SetActive(false);
        }
        else
        {
            Vector3 targetViewportPosition = mainCamera.WorldToViewportPoint(transform.position);

            if (targetViewportPosition.z > 0 && targetViewportPosition.x > 0 && targetViewportPosition.x < 1 && targetViewportPosition.y > 0 && targetViewportPosition.y < 1)
            {
                indicator.SetActive(false);
            }
            else
            {
                indicator.SetActive(true);
                Vector3 screenEdge = mainCamera.ViewportToWorldPoint(new Vector3(Mathf.Clamp(targetViewportPosition.x, 0.1f, 0.9f),
                    Mathf.Clamp(targetViewportPosition.y, 0.1f, 0.9f), mainCamera.nearClipPlane));

                indicator.transform.position = new Vector3(screenEdge.x, screenEdge.y, -9);
                RotateTowards(indicator.transform, transform.position);
            }
        }
    }

    private void RotateTowards(Transform transform, Vector2 target)
    {
        Vector2 direction = (target - (Vector2)transform.position).normalized;
        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        var offset = 90f;
        transform.rotation = Quaternion.Euler(Vector3.forward * (angle + offset));
    }
}
