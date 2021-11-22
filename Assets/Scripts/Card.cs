using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 卡带
/// </summary>
[CreateAssetMenu(fileName ="卡带", menuName ="卡带")]
public class Card : ScriptableObject
{

    /// <summary>
    /// 卡带名称
    /// </summary>
    public string CardName;

    /// <summary>
    /// 卡带图标
    /// </summary>
    public Sprite CardSprite;

    /// <summary>
    /// 插入后图标
    /// </summary>
    public Sprite PressSprite;

    /// <summary>
    /// 插入后悬浮图标
    /// </summary>
    public Sprite PressSpriteFloat;

    /// <summary>
    /// 信息面板
    /// </summary>
    public GameObject Inspector;

}
