using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dialog : MonoBehaviour
{
    /// <summary>
    /// 角色名控件
    /// </summary>
    [SerializeField]
    private Text _name;

    /// <summary>
    /// 正文控件
    /// </summary>
    [SerializeField]
    private Text _body;

    /// <summary>
    /// 正文
    /// </summary>
    private string _bodyText;

    /// <summary>
    /// 设置名字
    /// </summary>
    /// <param name="name"></param>
    public void SetName(string name)
    {
        _name.text = name;
    }

    /// <summary>
    /// 设置正文
    /// </summary>
    /// <param name="body"></param>
    /// <param name="show">是否直接显示</param>
    public void SetBody(string body, bool show = false)
    {

    }

    /// <summary>
    /// 逐字打印
    /// </summary>
    public void PopBody()
    {
        //Todo:实现PopBody
    }
}
