using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

public class FillSlider : SerializedMonoBehaviour
{
    [OdinSerialize, PropertyRange(0, 1)]
    public float Value
    {
        get => _value;
        set
        {
            _value = value;
            _fillImage.fillAmount = _value;
        }
    }

    private float _value;

    /// <summary>
    /// Ìî³äµÄÍ¼Æ¬
    /// </summary>
    [SerializeField]
    private Image _fillImage;
}
