using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// 冒泡式对话框
/// </summary>
public class BubbleDialog : Dialog
{

    /// <summary>
    /// 角色名控件
    /// </summary>
    [SerializeField]
    private TextMeshProUGUI _name;

    /// <summary>
    /// 跟随目标
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

    public void SetName(string newName)
    {
        _name.text = newName + ":";
    }
}
