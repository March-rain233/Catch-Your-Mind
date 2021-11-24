using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

/// <summary>
/// ������ʧ����
/// </summary>
public class FadeLine : SerializedMonoBehaviour
{
    [SerializeField]
    private LineRenderer _lineRenderer;

    /// <summary>
    /// ����ʱ��
    /// </summary>
    public float RemainTime;

    public Color StartColor;
    public Color EndColor;

    public float LineWidth;

    /// <summary>
    /// �Ƿ�����ڳ���
    /// </summary>
    [OdinSerialize]
    public bool IsRemain { get; private set; }

    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        IsRemain = false;
    }

    public void DoFade(Vector2[] vertexs)
    {
        _lineRenderer.positionCount = vertexs.Length;
        //_lineRenderer.loop = true;
        _lineRenderer.endWidth = _lineRenderer.startWidth = LineWidth;

        int i = 0;
        System.Array.ForEach(vertexs, vertex => _lineRenderer.SetPosition(i++, vertex));

        IsRemain = true;

        _lineRenderer.DOColor(new Color2(StartColor, StartColor), new Color2(EndColor, EndColor), RemainTime)
            .onComplete = () => IsRemain = false;
    }
}
