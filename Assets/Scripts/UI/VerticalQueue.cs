using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class VerticalQueue : MonoBehaviour
{
    private Queue<Transform> _transforms = new Queue<Transform>();
    private VerticalLayoutGroup _layoutGroup;

    public int MaxCount
    {
        get => _maxCount;
        set
        {
            _maxCount = value;
            while (Count > MaxCount)
            {
                Destroy(Dequeue(null).gameObject);
            }
        }
    }
    [SetProperty("MaxCount"), SerializeField]
    private int _maxCount;

    public int Count => _transforms.Count;

    public void Enqueue(Transform rect)
    {
        if (rect.parent != transform)
        {
            rect.SetParent(transform);
        }
        _transforms.Enqueue(rect);
        while (Count > MaxCount)
        {
            Destroy(Dequeue(null).gameObject);
        }
    }

    public Transform Dequeue(Transform pool)
    {
        var rect = _transforms.Dequeue();
        rect.SetParent(pool);
        return rect;
    }

    public Transform Peek()
    {
        return _transforms.Peek();
    }

    public Transform Last()
    {
        if (_transforms.Count <= 0) { return null; }
        return _transforms.Last();
    }
}
