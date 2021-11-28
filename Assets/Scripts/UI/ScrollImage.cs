using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class ScrollImage : MonoBehaviour, IScrollHandler
{
    public RectTransform RectTransform;
    public float Speed;
    public float Max;
    public float Min;

    private void Awake()
    {
        if (!RectTransform) { RectTransform = GetComponent<RectTransform>(); }
    }

    public void OnScroll(PointerEventData eventData)
    {
        float delta = eventData.scrollDelta.y * Speed;
        float y = RectTransform.localPosition.y;
        y = Mathf.Clamp(y + delta, Min, Max);
        RectTransform.DOLocalMoveY(y, 0.2f);
    }
}
