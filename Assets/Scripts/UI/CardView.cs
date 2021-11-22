using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CardView : MonoBehaviour, IDragHandler, IPointerClickHandler
{
    public Card Card;

    public RectTransform RectTransform
    {
        get
        {
            if (!_rectTransform)
            {
                _rectTransform = GetComponent<RectTransform>();
            }
            return _rectTransform;
        }
    }
    private RectTransform _rectTransform;

    /// <summary>
    /// µ±±»ÍÏ×§Ê±
    /// </summary>
    public System.Action<CardView, PointerEventData> OnDragged;
    public System.Action<CardView, PointerEventData> OnClicked;

    private void Awake()
    {
        GetComponent<Image>().sprite = Card.CardSprite;
    }

    public void OnDrag(PointerEventData eventData)
    {
        OnDragged?.Invoke(this, eventData);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnClicked?.Invoke(this, eventData);
    }
}
