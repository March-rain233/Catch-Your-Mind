using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

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

    public CanvasGroup CanvasGroup;
    public Image Next;

    /// <summary>
    /// 跟随目标
    /// </summary>
    public Transform Follow;

    public float _offsetX;

    public float _offsetY;

    private RectTransform _rectTransform;

    private Animator _animator;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _animator = GetComponent<Animator>();
        Next.DOFade(0, 0);
        BeginShow += () => Next.DOFade(0, 0);
        ShowEnd += () => Next.DOFade(1, 0.3f).SetLoops(-1, LoopType.Yoyo);
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

    public void Show()
    {
        CanvasGroup.DOFade(1, 0.1f);
    }
    public void Hide()
    {
        CanvasGroup.DOFade(0, 0.1f);
    }
}
