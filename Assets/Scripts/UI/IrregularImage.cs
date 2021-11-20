using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

public class IrregularImage : SerializedMonoBehaviour
{
    public Image Image;

    [OdinSerialize]
    public float AlphaHitTestMinimumThreshold
    {
        get
        {
            if(Image) return Image.alphaHitTestMinimumThreshold;
            return 0;
        }
        set
        {
            if (Image) Image.alphaHitTestMinimumThreshold = value;
        }
    }
}
