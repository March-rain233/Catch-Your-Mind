using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineDrawer : MonoBehaviour
{
    /// <summary>
    /// 鼠标
    /// </summary>
    [SerializeField]
    private Transform _cursor;
    /// <summary>
    /// 鼠标角度的偏移量
    /// </summary>
    private float _zOffset;
    /// <summary>
    /// 忽略改变的鼠标位移距离
    /// </summary>
    [SerializeField]
    private float _ignoreDistance;

    /// <summary>
    /// 线渲染器
    /// </summary>
    [SerializeField]
    private LineRenderer _lineRenderer;

    /// <summary>
    /// 碰撞体
    /// </summary>
    [SerializeField]
    private EdgeCollider2D _edgeCollider;

    /// <summary>
    /// 最高顶点数
    /// </summary>
    public int MaxVertexCount
    {
        get
        {
            return _maxVertexCount;
        }
        set
        {
            if(_maxVertexCount == value) { return; }
            else if(_maxVertexCount > value)
            {
                _vertexs.RemoveRange(0, value - _maxVertexCount);
                _lineRenderer.positionCount = value;
                Vector3[] temp = new Vector3[_vertexs.Count];
                for(int i = 0; i < _vertexs.Count; ++i)
                {
                    temp[i] = _vertexs[i];
                }
                _lineRenderer.SetPositions(temp);
            }
            _maxVertexCount = value;
            _lineRenderer.positionCount = _maxVertexCount;
        }
    }
    [SerializeField, SetProperty("MaxVertexCount")]
    private int _maxVertexCount;

    [SerializeField]
    /// <summary>
    /// 顶点生成的间隔距离
    /// </summary>
    private float _vertexDistance;

    /// <summary>
    /// 点集
    /// </summary>
    [SerializeField]
    private List<Vector2> _vertexs;

    /// <summary>
    /// 线的宽度
    /// </summary>
    public float LineWidth
    {
        get
        {
            return _lineWidth;
        }
        set
        {
            _lineWidth = value;
            _lineRenderer.endWidth = _lineRenderer.startWidth = value;
        }
    }
    [SerializeField, SetProperty("LineWidth")]
    private float _lineWidth;

    [SerializeField]
    /// <summary>
    /// 负责接受绘画的相机
    /// </summary>
    private Camera _camera;

    /// <summary>
    /// 曲线闭合事件
    /// </summary>
    public event System.Action<Vector2[]> LineClosed;
    /// <summary>
    /// 画线被打断事件
    /// </summary>
    public event System.Action DrawInterput;

    /// <summary>
    /// 是否正在画线
    /// </summary>
    private bool _drawing = false;

    /// <summary>
    /// 鼠标上一帧的位置
    /// </summary>
    private Vector2 _lastPoint;

    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _edgeCollider = GetComponent<EdgeCollider2D>();
        _vertexs = new List<Vector2>();

        Cursor.visible = false;
        _lastPoint = Vector2.zero;
    }

    // Update is called once per frame
    private void Update()
    {
        //改变鼠标朝向
        Vector2 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 dir = mouse - _lastPoint;
        _cursor.position = mouse;
        if (dir.magnitude >= _ignoreDistance)
        {
            _cursor.eulerAngles = new Vector3(0, 0, Vector2.SignedAngle(Vector2.up, dir));
            _lastPoint = mouse;
        }

        if (Input.GetMouseButtonDown(0))
        {
            BeginToDraw();
        }
        else if (_drawing && Input.GetMouseButton(0))
        {
            Draw();
        }
        if(Input.GetMouseButtonUp(0))
        {
            EndDraw();
        }
    }

    /// <summary>
    /// 开始绘画
    /// </summary>
    private void BeginToDraw()
    {
        AddPoint(_camera.ScreenToWorldPoint(Input.mousePosition));
        _edgeCollider.isTrigger = true;
        _drawing = true;
    }

    /// <summary>
    /// 绘画
    /// </summary>
    private void Draw()
    {
        var pos = _camera.ScreenToWorldPoint(Input.mousePosition);
        if(_vertexs.Count > 0 && (Vector2)pos == _vertexs[_vertexs.Count - 1]) { return; }

        ////判断是否形成封闭回路
        //var hit = Physics2D.OverlapCircle(pos, _vertexDistance * 2f);
        //if (hit != null)
        //{
        //    LineClose();
        //}
        //else
        if(!LineClose())
        {
            if(Vector2.Distance(pos, _vertexs[_vertexs.Count - 1]) < _vertexDistance)
            {
                return;
            }
            if(_vertexs.Count >= _maxVertexCount)
            {
                RemoveRange(0, _vertexs.Count - _maxVertexCount + 1);
            }
            AddPoint(pos);
        }
    }

    /// <summary>
    /// 停止绘画
    /// </summary>
    private void EndDraw()
    {
        RemoveRange(0, _vertexs.Count);
        _drawing = false;
    }

    /// <summary>
    /// 曲线封闭
    /// </summary>
    private bool LineClose()
    {
        if(_vertexs.Count <= 2) { return false; }

        Vector2 end = _vertexs[_vertexs.Count - 1];
        Vector2 start = _vertexs[_vertexs.Count - 2];
        //相交的线段的起点
        int saveIndex = -1;
        for(int i = 0; i < _vertexs.Count - 3; ++i)
        {
            Vector2 ts = _vertexs[i];
            Vector2 te = _vertexs[i + 1];
            if (RectClose(ts, te, start, end) && LineSegmentCross(ts, te, start, end))
            {
                saveIndex = i;
            }
        }
        if(saveIndex == -1) { return false; }

        Vector2 node = GetNode(_vertexs[saveIndex], _vertexs[saveIndex + 1], start, end);

        var closeLine = _vertexs.GetRange(saveIndex + 1, _vertexs.Count - saveIndex - 1);
        closeLine.Insert(0, node);

        RemoveRange(saveIndex + 1, _vertexs.Count);
        AddPoint(node);

        _edgeCollider.isTrigger = true;
        LineClosed?.Invoke(closeLine.ToArray());
        return true;
    }

    /// <summary>
    /// 快速排斥
    /// </summary>
    /// <param name="p1"></param>
    /// <param name="p2"></param>
    /// <param name="q1"></param>
    /// <param name="q2"></param>
    /// <returns></returns>
    private bool RectClose(Vector2 p1, Vector2 p2, Vector2 q1, Vector2 q2)
    {
        return Mathf.Min(p1.x, p2.x) <= Mathf.Max(q1.x, q2.x)
            && Mathf.Min(q1.x, q2.x) <= Mathf.Max(p1.x, p2.x)
            && Mathf.Min(p1.y, p2.y) <= Mathf.Max(q1.y, q2.y)
            && Mathf.Min(q1.y, q2.y) <= Mathf.Max(p1.y, p2.y);
    }

    /// <summary>
    /// 跨立测试
    /// </summary>
    /// <param name="p1"></param>
    /// <param name="p2"></param>
    /// <param name="q1"></param>
    /// <param name="q2"></param>
    /// <returns></returns>
    private bool LineSegmentCross(Vector2 p1, Vector2 p2, Vector2 q1, Vector2 q2)
    {
        double u, v, w, z;
        u = (q1.x - p1.x) * (p2.y - p1.y) - (p2.x - p1.x) * (q1.y - p1.y);
        v = (q2.x - p1.x) * (p2.y - p1.y) - (p2.x - p1.x) * (q2.y - p1.y);
        w = (p1.x - q1.x) * (q2.y - q1.y) - (q2.x - q1.x) * (p1.y - q1.y);
        z = (p2.x - q1.x) * (q2.y - q1.y) - (q2.x - q1.x) * (p2.y - q1.y);
        return (u * v <= 0.00000001 && w * z <= 0.00000001);
    }

    /// <summary>
    /// 获取线段交点
    /// </summary>
    /// <param name="p1"></param>
    /// <param name="p2"></param>
    /// <param name="q1"></param>
    /// <param name="q2"></param>
    /// <returns></returns>
    private Vector2 GetNode(Vector2 p1, Vector2 p2, Vector2 q1, Vector2 q2)
    {
        float b1 = (p2.y - p1.y) * p1.x + (p1.x - p2.x) * p1.y;
        float b2 = (q2.y - q1.y) * q1.x + (q1.x - q2.x) * q1.y;
        float d = (p2.x - p1.x) * (q2.y - q1.y) - (q2.x - q1.x) * (p2.y - p1.y);
        float d1 = b2 * (p2.x - p1.x) - b1 * (q2.x - q1.x);
        float d2 = b2 * (p2.y - p1.y) - b1 * (q2.y - q1.y);
        return new Vector2(d1 / d, d2 / d);
    }

    /// <summary>
    /// 添加点
    /// </summary>
    /// <param name="point"></param>
    private void AddPoint(Vector2 point)
    {
        _vertexs.Add(point);
        _lineRenderer.positionCount = _vertexs.Count;
        _lineRenderer.SetPosition(_vertexs.Count - 1, point);
        _edgeCollider.SetPoints(_vertexs);
    }

    /// <summary>
    /// 移除[start, end)的点
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    private void RemoveRange(int start, int end)
    {
        _vertexs.RemoveRange(start, end - start);
        Vector3[] temp = new Vector3[_vertexs.Count];
        _lineRenderer.positionCount = _vertexs.Count;
        for (int i = 0; i < _vertexs.Count; ++i)
        {
            temp[i] = _vertexs[i];
            _lineRenderer.SetPosition(i, _vertexs[i]);
        }
        _edgeCollider.Reset();
        _edgeCollider.SetPoints(_vertexs);
    }

    /// <summary>
    /// 打断画线
    /// </summary>
    public void Interrupt()
    {
        EndDraw();
        DrawInterput?.Invoke();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Interrupt();
    }
}
