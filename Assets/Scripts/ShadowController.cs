using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ShadowController : MonoBehaviour
{
    /// <summary>
    /// 存在时间
    /// </summary>
    [SerializeField]
    public float ExistTime;

    [SerializeField]
    public Color ShadowColor;

    private SpriteRenderer _spriteRenderer;

    private Queue<SpriteRenderer> _shadowPool = new Queue<SpriteRenderer>();

    [SerializeField]
    public float Interval;

    private float _lastCreate;

    /// <summary>
    /// 是否创建残影
    /// </summary>
    public bool IsCreate;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (IsCreate && Time.time - _lastCreate >= Interval)
        {
            _lastCreate = Time.time;
            Create();
        }
    }

    private void Create()
    {
        if (_shadowPool.Count <= 0)
        {
            var obj = new GameObject("shadow", typeof(SpriteRenderer));
            _shadowPool.Enqueue(obj.GetComponent<SpriteRenderer>());
        }
        var sr = _shadowPool.Dequeue();
        sr.transform.SetParent(null);
        sr.transform.position = transform.position;
        sr.gameObject.SetActive(true);
        sr.sprite = _spriteRenderer.sprite;
        sr.color = ShadowColor;
        sr.sortingLayerID = _spriteRenderer.sortingLayerID;
        sr.sortingOrder = _spriteRenderer.sortingOrder - 1;
        sr.DOFade(0, ExistTime).onComplete = () =>
        {
            _shadowPool.Enqueue(sr);
            sr.gameObject.SetActive(false);
            sr.transform.SetParent(transform);
        };
    }
}
