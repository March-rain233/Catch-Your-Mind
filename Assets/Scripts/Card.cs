using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ����
/// </summary>
[CreateAssetMenu(fileName ="����", menuName ="����")]
public class Card : ScriptableObject
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
    public GameObject Inspector;

}
