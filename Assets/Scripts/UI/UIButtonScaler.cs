using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIButtonScaler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Action<int> OnAnimateIn;
    public Action<int> OnAnimateOut;

    [SerializeField] private Transform activeTransform;
    [SerializeField] private float ScaleTime = 0.15f;
    [SerializeField] private float ScaleMin = 0.85f;
    [SerializeField] private float ScaleMax = 1;

    [SerializeField] private bool separate = false;
    [SerializeField] private Vector3 ScaleMinVec = new Vector3(0.85f, 0.85f, 1);
    [SerializeField] private Vector3 ScaleMaxVec = new Vector3(1, 1, 1);

    private LTDescr activeTween;
    private int indexForEvent = -1;
    private bool animEnabled = true;

    private void Awake()
    {
        if (activeTransform == null)
        {
            activeTransform = transform;
        }
    }

    public void EnableScaling(bool enable = true)
    {
        animEnabled = enable;

        if (!animEnabled)
        {
            CancelActiveTween();
            activeTween = activeTransform.LeanScale(separate ? ScaleMaxVec : Vector3.one * ScaleMax, ScaleTime).setEaseOutBack().setOnComplete(() =>
            {
                OnAnimateOut?.Invoke(indexForEvent);
            });
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!animEnabled) return;

        OnAnimateIn?.Invoke(indexForEvent);
        CancelActiveTween(true);
        activeTween = activeTransform.LeanScale(separate ? ScaleMinVec : Vector3.one * ScaleMin, ScaleTime).setEaseOutBack();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!animEnabled) return;

        CancelActiveTween();
        activeTween = activeTransform.LeanScale(separate ? ScaleMaxVec : Vector3.one * ScaleMax, ScaleTime).setEaseOutBack().setOnComplete(() =>
        {
            OnAnimateOut?.Invoke(indexForEvent);
        });

    }

    public void SetIndex(int index)
    {
        indexForEvent = index;
    }


    public void CancelActiveTween(bool resetScale = false)
    {
        if (resetScale)
        {
            activeTransform.localScale = separate ? ScaleMaxVec : Vector3.one * ScaleMax;
        }

        if (activeTween != null)
        {
            LeanTween.cancel(activeTransform.gameObject, activeTween.uniqueId);
        }
    }
}

