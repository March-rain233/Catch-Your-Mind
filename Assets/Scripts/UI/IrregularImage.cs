using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

public class IrregularImage : MonoBehaviour
{
    public Image Image;

    public float AlphaHitTestMinimumThreshold;

    public void Awake()
    {
        Image.alphaHitTestMinimumThreshold = AlphaHitTestMinimumThreshold;
    }
}
