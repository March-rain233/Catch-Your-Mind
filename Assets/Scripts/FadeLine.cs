using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

/// <summary>
/// 会逐渐消失的线
/// </summary>
public class FadeLine : SerializedMonoBehaviour
{
    [SerializeField]
    private LineRenderer _lineRenderer;

    /// <summary>
    /// 残留时间
    /// </summary>
    public float RemainTime;

    public Color StartColor;
    public Color EndColor;

    public float LineWidth;

    /// <summary>
    /// 是否残留在场上
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
