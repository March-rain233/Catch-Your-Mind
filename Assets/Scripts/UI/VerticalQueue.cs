using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class VerticalQueue : MonoBehaviour
{
    private Queue<RectTransform> _rectTransforms = new Queue<RectTransform>();
    private VerticalLayoutGroup _layoutGroup;

    public int MaxCount
    {
        get => _maxCount;
        set
        {
            _maxCount = value;
            while (Count > MaxCount)
            {
                Destroy(Dequeue(null));
            }
        }
    }
    private int _maxCount;

    public int Count => _rectTransforms.Count;

    public void Enqueue(RectTransform rect)
    {
        rect.SetParent(transform);
        _rectTransforms.Enqueue(rect);
        while (Count > MaxCount)
        {
            Destroy(Dequeue(null));
        }
    }

    public RectTransform Dequeue(Transform pool)
    {
        var rect = _rectTransforms.Dequeue();
        rect.SetParent(pool);
        return rect;
    }

    public RectTransform Peek()
    {
        return _rectTransforms.Peek();
    }

    public RectTransform Last()
    {
        return _rectTransforms.Last();
    }
}
