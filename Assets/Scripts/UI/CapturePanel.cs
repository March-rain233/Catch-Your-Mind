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
    /// 最大血量
    /// </summary>
    public float MaxBlood;

    /// <summary>
    /// 每次上升的血量
    /// </summary>
    public float UpBlood;
    /// <summary>
    /// 每次下降的血量
    /// </summary>
    public float DownBlood;

    /// <summary>
    /// 下降需要的时间
    /// </summary>
    public float WaitTime;
    /// <summary>
    /// 上次封闭的时间
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
