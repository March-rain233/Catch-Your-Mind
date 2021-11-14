using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dialog : MonoBehaviour
{
    /// <summary>
    /// ��ɫ���ؼ�
    /// </summary>
    [SerializeField]
    private Text _name;

    /// <summary>
    /// ���Ŀؼ�
    /// </summary>
    [SerializeField]
    private Text _body;

    /// <summary>
    /// ����
    /// </summary>
    private string _bodyText;

    /// <summary>
    /// ��������
    /// </summary>
    /// <param name="name"></param>
    public void SetName(string name)
    {
        _name.text = name;
    }

    /// <summary>
    /// ��������
    /// </summary>
    /// <param name="body"></param>
    /// <param name="show">�Ƿ�ֱ����ʾ</param>
    public void SetBody(string body, bool show = false)
    {

    }

    /// <summary>
    /// ���ִ�ӡ
    /// </summary>
    public void PopBody()
    {
        //Todo:ʵ��PopBody
    }
}
