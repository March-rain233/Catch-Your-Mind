using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class 心动培养基 : MonoBehaviour
{
    public CanvasGroup Default;
    public CanvasGroup Yue;
    public MyButton MyButton;
    public Card Card;

    private void Awake()
    {
        if(HaveTag("标签纸"))
        {
            if (!HaveTag("打开心动培养基"))
            {
                GameManager.Instance.EventCenter.SendEvent("心动培养基-拿到标签后（初次）", new EventCenter.EventArgs() { Boolean = true });
            }
            else
            {
                GameManager.Instance.EventCenter.SendEvent("心动培养基-拿到标签后", new EventCenter.EventArgs() { Boolean = true });
            }
        }
        else
        {
            if (!HaveTag("打开心动培养基"))
            {
                GameManager.Instance.EventCenter.SendEvent("心动培养基-没拿标签", new EventCenter.EventArgs() { Boolean = true });
            }
        }

        if (HaveTag("心动培养基更新内容"))
        {
            Yue.alpha = 1;
            Yue.blocksRaycasts = true;
            Default.alpha = 0;
            Default.blocksRaycasts = false;

            Card.name = "心动培养基";
        }
        else
        {
            Yue.alpha = 0;
            Yue.blocksRaycasts = false;
            Default.alpha = 1;
            Default.blocksRaycasts = true;

            Card.name = "缺少标签的心动培养基";
        }

        if (HaveTag("心动培养基插入解禁"))
        {
            EnableCard();
        }
        else
        {
            Card.TruthUnable = true;
            Card.TrueState = false;
            if (HaveTag("禁用心动培养基"))
            {
                Card.InsertUnable = true;
            }
            else
            {
                Card.InsertUnable = false;
            }
        }

        MyButton.OnClick += MyButton_OnClick; ;
        GameManager.Instance.EventCenter.AddListener("缺少标签的心动培养基插入", 随便叫什么都好);
        GameManager.Instance.EventCenter.AddListener("禁用心动培养基", 叫什么都好);
        GameManager.Instance.EventCenter.AddListener("心动培养基更新内容", WhatEver);
        GameManager.Instance.EventCenter.AddListener("心动培养基插入解禁", MyFunc);
        GameManager.Instance.EventCenter.SendEvent("打开心动培养基", new EventCenter.EventArgs() { Boolean = true });
    }

    private void MyButton_OnClick()
    {
        GameManager.Instance.EventCenter.SendEvent("调查营养源", new EventCenter.EventArgs() { Boolean = true });
    }

    private void MyFunc(EventCenter.EventArgs eventArgs)
    {
        EnableCard();
    }

    private void OnDestroy()
    {
        GameManager.Instance.EventCenter.RemoveListener("心动培养基插入解禁", MyFunc);
        GameManager.Instance.EventCenter.RemoveListener("缺少标签的心动培养基插入", 随便叫什么都好);
        GameManager.Instance.EventCenter.RemoveListener("禁用心动培养基", 叫什么都好);
        GameManager.Instance.EventCenter.RemoveListener("心动培养基更新内容", WhatEver);
    }

    public bool HaveTag(string name)
    {
        return GameManager.Instance.EventCenter.TryGetEventArgs(name, out EventCenter.EventArgs eventArgs) && eventArgs.Boolean;
    }

    private void EnableCard()
    {
        Card.TruthUnable = false;
        Card.InsertUnable = false;
        Card.TrueState = true;
    }

    private void WhatEver(EventCenter.EventArgs eventArgs)
    {
        EnableCard();
        Default.DOFade(0, 0.2f).onComplete = () =>
         {
             Default.blocksRaycasts = false;
             Yue.DOFade(1, 0.2f);
             Yue.blocksRaycasts = true;
         };
    }

    private void 叫什么都好(EventCenter.EventArgs eventArgs)
    {
        if (HaveTag("心动培养基插入解禁")) { return; }
        Card.TruthUnable = true;
        Card.InsertUnable = true;
    }

    private void 随便叫什么都好(EventCenter.EventArgs eventArgs)
    {
        if (HaveTag("心动培养基插入解禁")) { return; }
        GameManager.Instance.EventCenter.SendEvent("心动培养基-插入-没对象", new EventCenter.EventArgs() { Boolean = true });
    }
}
