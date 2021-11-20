using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VerticalQueue : MonoBehaviour
{
    private Queue<RectTransform> _rectTransforms = new Queue<RectTransform>();
    private VerticalLayoutGroup _layoutGroup;
    public int Count => _rectTransforms.Count;

    public void Enqueue(RectTransform rect)
    {
        rect.SetParent(transform);
        _rectTransforms.Enqueue(rect);
    }

    public RectTransform Dequeue(Transform pool)
    {
        var rect = _rectTransforms.Dequeue();
        rect.SetParent(pool);
        return rect;
    }
}
