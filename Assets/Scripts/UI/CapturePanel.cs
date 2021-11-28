using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using DG.Tweening;
using UnityEngine.UI;


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
    /// 每次上升的基础血量
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

    /// <summary>
    /// 被打断的惩罚
    /// </summary>
    /// <remarks>
    /// 即扣除的时间数值
    /// </remarks>
    public float InterruptPunish;

    /// <summary>
    /// 当前连击数
    /// </summary>
    [SerializeField]
    private int _combo;
    /// <summary>
    /// 最大有效连击数
    /// </summary>
    public int MaxEffectiveCombo;
    /// <summary>
    /// 最小有效连击数
    /// </summary>
    /// <remarks>
    /// 即最小的开始增长倍率的连击数
    /// </remarks>
    public int MinEffectiveCombo;
    /// <summary>
    /// 最大连击倍率
    /// </summary>
    public float MaxComboRate;

    public CanvasGroup ComboPanel;
    public Text ComboNum;

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

    public NPC.BehaviorTreeRunner Target;
    public PolygonCollider2D PolygonCollider2D;

    [SerializeField]
    private List<FadeLine> _lines;
    [SerializeField]
    private GameObject _fadeLinePrefabs;

    private void Awake()
    {
        LineDrawer.LineClosed += JudgeLineClose;
        LineDrawer.DrawInterput += InterruptHandler;

        GameManager.Instance.TimeChanged += time =>
        {
            TimeView.Value = time / GameManager.Instance.MaxTime;
        };
        Blood = 0;
        _lastClose = Time.time;
        ComboPanel.alpha = 0;
    }

    private void Update()
    {
        if(Time.time - _lastClose>= WaitTime && Blood > 0)
        {
            Blood -= DownBlood*Time.deltaTime;
        }
    }

    private void InterruptHandler()
    {
        GameManager.Instance.RemainTime -= InterruptPunish;
        _combo = 0;
        ComboPanel.DOFade(0, 0.2f);
        ComboNum.text = _combo.ToString();
    }

    private void JudgeLineClose(Vector2[] obj)
    {
        PolygonCollider2D.points = obj;
        Bounds bounds = PolygonCollider2D.bounds;
        //System.Array.ForEach(obj, node => bounds.Encapsulate(node));
        if (bounds.Contains(Target.transform.position) && Target.Variables["IsCatchable"].Boolean)
        {
            CreateFadeLine(obj);
            Blood += UpBlood * Mathf.Lerp(1, MaxComboRate, Mathf.Clamp(_combo, MinEffectiveCombo, MaxEffectiveCombo) - MinEffectiveCombo);
            _lastClose = Time.time;
            ++_combo;
            if (_combo >= 2 && ComboPanel.alpha == 0)
            {
                ComboPanel.DOFade(1, 0.5f);
            }
            ComboNum.text = _combo.ToString();
        }
    }

    private void CreateFadeLine(Vector2[] vertexs)
    {
        var line = _lines.Find(line => { return !line.IsRemain; });
        if (!line)
        {
            line = Instantiate(_fadeLinePrefabs).GetComponent<FadeLine>();
            _lines.Add(line);
        }
        line.DoFade(vertexs);
    }
}
