using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 视图的基类，作为View层管控窗口的显示
/// </summary>
public abstract class BasePanel : BaseView
{
    public abstract PanelType Type
    {
        get;
    }
}


/// <summary>
/// 窗口类型
/// </summary>
[System.Serializable]
public enum PanelType
{
    StatusPanel,
}