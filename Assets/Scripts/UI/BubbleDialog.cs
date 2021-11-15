using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ð��ʽ�Ի���
/// </summary>
public class BubbleDialog : Dialog
{
    /// <summary>
    /// ����Ŀ��
    /// </summary>
    public Transform Follow;

    public float _offsetX;

    public float _offsetY;

    private RectTransform _rectTransform;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    private void FixedUpdate()
    {
        if (Follow)
        {
            var position = Camera.main.WorldToScreenPoint(Follow.position);
            position.x += _offsetX;
            position.y += _offsetY;
            _rectTransform.anchoredPosition = position;
        }
    }
}
