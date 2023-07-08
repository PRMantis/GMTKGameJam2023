using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Indicator : MonoBehaviour
{
    [SerializeField] private float offScreenThreshold = 10f;
    [SerializeField] private GameObject indicatorPrefab;
    [SerializeField] private Player player;

    private GameObject indicator;
    private bool isIndicatorActive;
    private Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main;
        indicator = Instantiate(indicatorPrefab);
        indicator.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetDirection = transform.position - player.transform.position;
        float distanceToTarget = targetDirection.magnitude;

        if (distanceToTarget < offScreenThreshold)
        {
            indicator.SetActive(false);
            isIndicatorActive = false;
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
                isIndicatorActive = true;
                indicator.SetActive(true);
                Vector3 screenEdge = mainCamera.ViewportToWorldPoint(new Vector3(Mathf.Clamp(targetViewportPosition.x, 0.1f, 0.9f),
                    Mathf.Clamp(targetViewportPosition.y, 0.1f, 0.9f), mainCamera.nearClipPlane));

                indicator.transform.position = new Vector3(screenEdge.x, screenEdge.y, -9);
                RotateTowards(indicator.transform, transform.position);
                //Quaternion rotation = Quaternion.LookRotation(transform.position - indicator.transform.position,
                //    indicator.transform.TransformDirection(Vector3.up));
                //indicator.transform.rotation = new Quaternion(0, 0, rotation.z, rotation.w);
                //indicator.transform.LookAt(transform.position, transform.up);
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
