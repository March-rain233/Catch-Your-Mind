using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FloatInfo : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private Vector2 _oriPosition;

    public Transform Target;

    [SerializeField]
    private bool _isMouseEnter;

    [SerializeField]
    private float _vector;

    private void Awake()
    {
        _oriPosition = transform.localPosition;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _isMouseEnter = true;
        transform.DOKill();
        transform.DOLocalMoveX(Target.transform.localPosition.x, _vector).SetSpeedBased();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _isMouseEnter = false;
        transform.DOKill();
        transform.DOLocalMoveX(_oriPosition.x, _vector).SetSpeedBased();
    }
}
