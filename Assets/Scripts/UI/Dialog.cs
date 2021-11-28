using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.Serialization;
using Sirenix.OdinInspector;
using System.Text.RegularExpressions;

public abstract class Dialog : SerializedMonoBehaviour
{
    /// <summary>
    /// 正文控件
    /// </summary>
    [SerializeField]
    protected TextMeshProUGUI _textLable;

    /// <summary>
    /// 当前输出的文本
    /// </summary>
    [SerializeField, TextArea]
    protected string _outPutText;

    /// <summary>
    /// 当前文本速率
    /// </summary>
    [SerializeField]
    protected float _textSpeed;

    /// <summary>
    /// 默认输出速率
    /// </summary>
    [SerializeField]
    protected float _defaultSpeed;

    /// <summary>
    /// 当前是否在输出
    /// </summary>
    [OdinSerialize]
    public bool Typing
    {
        get;
        private set;
    }

    //判断是否为标签
    protected static Regex _open = new Regex(@"^<[^/]+?>");
    protected static Regex _close = new Regex(@"^</[^/]+?>");

    /// <summary>
    /// 当前支持的自定义标签
    /// </summary>
    [OdinSerialize]
    protected static List<string> UserTag = new List<string>();

    public System.Action ShowEnd;
    public System.Action BeginShow;

    /// <summary>
    /// 逐字输出
    /// </summary>
    /// <returns></returns>
    private IEnumerator ReadWord()
    {
        string tempSave = _outPutText;

        int offset = 0;
        //逐字输出至文本框
        for (int i = 0; i < tempSave.Length; ++i)
        {
            while (Regex.IsMatch(tempSave.Substring(i), @"^<.+?>"))
            {
                //判断是否为开标签，如果是则进行专门处理
                if (_open.IsMatch(tempSave.Substring(i)))
                {
                    var tag = _open.Match(tempSave.Substring(i)).Value;
                    //对不属于html的自定义标签进行特殊处理
                    if (IsUserTag(tag))
                    {
                        //执行标签功能
                        UserOpenTagHandler(tag);

                        //从需要输出的字符串中删去
                        tempSave = _open.Replace(tempSave, "", 1);
                    }
                    else
                    {
                        //将开标签插入并把光标置后
                        _textLable.text = _textLable.text.Insert(offset + i, tag);
                        i += tag.Length;

                        //生成对应闭标签并插入
                        string close = Regex.Match(tag
                            , @"<[^/]+?>").Value
                            .Insert(1, @"/");
                        _textLable.text = _textLable.text.Insert(offset + i, close);
                    }
                }
                else if (_close.IsMatch(tempSave.Substring(i)))
                {
                    var tag = _close.Match(tempSave.Substring(i)).Value;
                    if (IsUserTag(tag))
                    {
                        //执行标签功能
                        UserCloseTagHandler(tag);

                        //从需要输出的字符串中删去
                        tempSave = _close.Replace(tempSave, "", 1);
                    }
                    else
                    {
                        i += _close.Match(tempSave.Substring(i)).Value.Length;
                    }
                }
            }
            if (i < tempSave.Length)
            {
                _textLable.text = _textLable.text.Insert(offset + i, _outPutText[i].ToString());
            }
            yield return new WaitForSeconds(_textSpeed);
        }

        //输出完毕，通知控制器输出下一行
        ReadFin();
    }

    /// <summary>
    /// 判断是否为用户自定义的标签
    /// </summary>
    protected static bool IsUserTag(string source)
    {
        //获取标签关键字
        source = Regex.Match(source
            , @"(?<=\<)(/?[^/= ]+?)(?=(( |=)[^>]*?)?\>)").Value;

        //遍历存储的用户标签，查看是否匹配
        return UserTag.Exists(tag => tag == source);
    }

    /// <summary>
    /// 根据传入的自定义标签头执行对应逻辑
    /// </summary>
    protected virtual void UserOpenTagHandler(string value)
    {
        //goto:标签头
    }

    /// <summary>
    /// 根据传入的自定义标签尾执行对应逻辑
    /// </summary>
    protected virtual void UserCloseTagHandler(string value)
    {
        //TODO:封闭标签
    }

    /// <summary>
    /// 获取最终显示的文本
    /// </summary>
    /// <returns></returns>
    protected string GetOutPutText()
    {
        string tempSave = _outPutText;
        for (int i = 0; i < tempSave.Length; ++i)
        {
            //把所有的自定义标签都转发为空
            while (i < tempSave.Length
                && Regex.IsMatch(tempSave.Substring(i), @"^<.+?>"))
            {
                string tag = Regex.Match(tempSave.Substring(i)
                    , @"^<.+?>").Value;
                if (IsUserTag(tag))
                {
                    tempSave.Replace(tag, "");
                }
                else { break; }
            }
        }
        return tempSave;
    }

    /// <summary>
    /// 开始输出
    /// </summary>
    /// <param name="output"></param>
    [Button]
    public void BeginRead(string output)
    {
        StopAllCoroutines();
        _textLable.text = "";
        Typing = true;
        _outPutText = output;
        BeginShow?.Invoke();
        StartCoroutine("ReadWord");
    }

    public void OutputImmediately()
    {
        StopAllCoroutines();
        _textLable.text = "";
        BeginShow?.Invoke();
        ReadFin();
    }

    /// <summary>
    /// 立即输出至屏幕
    /// </summary>
    /// <param name="output"></param>
    public void OutputImmediately(string output)
    {
        StopAllCoroutines();
        _textLable.text = "";
        _outPutText = output;
        BeginShow?.Invoke();
        ReadFin();
    }

    /// <summary>
    /// 输出完成
    /// </summary>
    private void ReadFin()
    {
        _textLable.text = GetOutPutText();
        Typing = false;
        ShowEnd.Invoke();
    }
}
