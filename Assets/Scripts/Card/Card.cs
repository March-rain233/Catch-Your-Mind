using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

/// <summary>
/// 卡带
/// </summary>
[CreateAssetMenu(fileName ="卡带", menuName ="卡带")]
public class Card : SerializedScriptableObject
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
    public GameObject DefaultInspector;

    public Dictionary<Condition, GameObject> Inspectors;

    /// <summary>
    /// 插入按钮是否显示为禁用
    /// </summary>
    public bool InsertUnable;

    /// <summary>
    /// 是否真的无法插♂入
    /// </summary>
    public bool TruthUnable;

    /// <summary>
    /// 卡带选择是否正确
    /// </summary>
    public bool TrueState;

    public virtual bool Insert()
    {
        GameManager.Instance.EventCenter.SendEvent($"{CardName}插入", new EventCenter.EventArgs() { Boolean = true });
        return !TruthUnable;
    }

    public virtual void Open()
    {
        GameManager.Instance.EventCenter.SendEvent(CardName, new EventCenter.EventArgs() { Boolean = true });
    }

    public GameObject GetInspector()
    {
        GameObject t = DefaultInspector;
        if (Inspectors == null || Inspectors.Count <= 0)
        {
            return t;
        }
        foreach(var i in Inspectors)
        {
            if (i.Key.Reason()) 
            { 
                t = i.Value; 
                return t; 
            }
        }
        return t;
    }
}
