using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 窗口的Control层基类，全为单例模式
/// </summary>
public abstract class BaseControl<T> where T:new ()
{
    private static T _instance;
    /// <summary>
    /// 单例访问点
    /// </summary>
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new T();
            }
            return _instance;
        }
    }

    /// <summary>
    /// UI运行入口
    /// </summary>
    /// <remarks>
    /// 用于订阅view的OnEnter事件，将把view和model绑定
    /// </remarks>
    public abstract void OnEnter(BaseView view);

    /// <summary>
    /// UI运行出口
    /// </summary>
    /// <remarks>
    /// 用于订阅view的OnExit事件，将解绑view和model
    /// </remarks>
    public abstract void OnExit(BaseView view);
}