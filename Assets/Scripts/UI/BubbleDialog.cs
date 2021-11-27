using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// ð��ʽ�Ի���
/// </summary>
public class BubbleDialog : Dialog
{

    /// <summary>
    /// ��ɫ���ؼ�
    /// </summary>
    [SerializeField]
    private TextMeshProUGUI _name;

    /// <summary>
    /// ����Ŀ��
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

    }
    public void Hide()
    {
        
    }
}
