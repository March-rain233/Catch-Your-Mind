using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class CapturePanel : MonoBehaviour
{
    public LineDrawer LineDrawer;

    public FillSlider BloodView;
    public FillSlider TimeView;

    /// <summary>
    /// ���Ѫ��
    /// </summary>
    public float MaxBlood;

    /// <summary>
    /// ÿ��������Ѫ��
    /// </summary>
    public float UpBlood;
    /// <summary>
    /// ÿ���½���Ѫ��
    /// </summary>
    public float DownBlood;

    /// <summary>
    /// �½���Ҫ��ʱ��
    /// </summary>
    public float WaitTime;
    /// <summary>
    /// �ϴη�յ�ʱ��
    /// </summary>
    private float _lastClose;

    public float Blood
    {
        get => _blood;
        set
        {
            _blood = value;
            BloodView.Value = _blood / MaxBlood;
        }
    }
    private float _blood;

    public Transform Target;
    public PolygonCollider2D PolygonCollider2D;

    private void Awake()
    {
        LineDrawer.LineClosed += JudgeLineClose;
        GameManager.Instance.TimeChanged += time =>
        {
            TimeView.Value = time / GameManager.Instance.MaxTime;
        };
        Blood = 0;
        _lastClose = Time.time;
    }

    private void Update()
    {
        if(Time.time - _lastClose>= WaitTime && Blood > 0)
        {
            Blood -= DownBlood*Time.deltaTime;
        }
    }

    private void JudgeLineClose(Vector2[] obj)
    {
        PolygonCollider2D.points = obj;
        Bounds bounds = PolygonCollider2D.bounds;
        //System.Array.ForEach(obj, node => bounds.Encapsulate(node));
        if (bounds.Contains(Target.position))
        {
            Blood += UpBlood;
            _lastClose = Time.time;
        }
    }
}
