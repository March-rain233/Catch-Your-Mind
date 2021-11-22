using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

public class FillSlider : SerializedMonoBehaviour
{
    [PropertyRange(0, 1)]
    public float Value
    {
        get => _value;
        set
        {
            _value = value;
            _fillImage.fillAmount = _value;
        }
    }
    [SerializeField, SetProperty("Value")]
    private float _value;

    /// <summary>
    /// ����ͼƬ
    /// </summary>
    [SerializeField]
    private Image _fillImage;
}
