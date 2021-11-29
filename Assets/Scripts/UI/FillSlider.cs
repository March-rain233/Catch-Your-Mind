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
            _value = Mathf.Lerp(MinOffset, MaxOffset, value);
            if(_fillImage) _fillImage.fillAmount = _value;
        }
    }
    [SerializeField, SetProperty("Value")]
    private float _value;

    /// <summary>
    /// Ìî³äµÄÍ¼Æ¬
    /// </summary>
    [SerializeField]
    private Image _fillImage;

    public float MinOffset = 0;
    public float MaxOffset = 1;
}
