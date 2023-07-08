using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class DynamicText : MonoBehaviour
{
    [SerializeField] private TMP_Text textObject;

    private LTDescr textMovingDescr;
    private Transform mainCamera;

    private void Awake()
    {
        transform.localScale = Vector3.zero;
        mainCamera = Camera.main.transform;
    }


    void Start()
    {

    }

    private void Update()
    {
        if (gameObject.activeSelf)
        {
            transform.rotation = Quaternion.LookRotation(transform.position - mainCamera.position);
        }
    }

    public void StartAnimation(Vector3 pos, float animTime)
    {
        transform.localScale = Vector3.zero;
        transform.position = pos;

        transform.LeanScale(Vector3.one, animTime / 2).setEaseOutBack();

        transform.LeanMove(pos, animTime / 2).setEaseOutCirc().setOnComplete(() =>
        {
            transform.LeanScale(Vector3.zero, animTime / 2).setEaseInBack().setOnComplete(() =>
            {
                transform.localScale = Vector3.zero;
                DynamicWorldText.Instance.Pool.Release(this);
            });
        });
    }

    public void SetValues(Color color, string text)
    {
        textObject.color = color;
        textObject.text = text;
    }

    private void CancelTween(GameObject gameObject, LTDescr descr)
    {
        if (gameObject != null)
        {
            LeanTween.cancel(gameObject, descr.id);
        }
    }
}
