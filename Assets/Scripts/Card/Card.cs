using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

/// <summary>
/// ����
/// </summary>
[CreateAssetMenu(fileName ="����", menuName ="����")]
public class Card : SerializedScriptableObject
{

    /// <summary>
    /// ��������
    /// </summary>
    public string CardName;

    /// <summary>
    /// ����ͼ��
    /// </summary>
    public Sprite CardSprite;

    /// <summary>
    /// �����ͼ��
    /// </summary>
    public Sprite PressSprite;

    /// <summary>
    /// ���������ͼ��
    /// </summary>
    public Sprite PressSpriteFloat;

    /// <summary>
    /// ��Ϣ���
    /// </summary>
    public GameObject DefaultInspector;

    public Dictionary<Condition, GameObject> Inspectors;

    /// <summary>
    /// ���밴ť�Ƿ���ʾΪ����
    /// </summary>
    public bool InsertUnable;

    /// <summary>
    /// �Ƿ�����޷������
    /// </summary>
    public bool TruthUnable;

    /// <summary>
    /// ����ѡ���Ƿ���ȷ
    /// </summary>
    public bool TrueState;

    public virtual bool Insert()
    {
        GameManager.Instance.EventCenter.SendEvent($"{CardName}����", new EventCenter.EventArgs() { Boolean = true });
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
