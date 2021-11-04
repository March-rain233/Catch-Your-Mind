using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��ͼ�Ļ��࣬��ΪView��ܿش��ڵ���ʾ
/// </summary>
public abstract class BasePanel : BaseView
{
    public abstract WindowType Type
    {
        get;
    }
}


/// <summary>
/// ��������
/// </summary>
[System.Serializable]
public enum WindowType
{
    Menu,
    Setting,
    StartMenu,
    introduction,
    loading,
    StatusUI,
    Dialog
}