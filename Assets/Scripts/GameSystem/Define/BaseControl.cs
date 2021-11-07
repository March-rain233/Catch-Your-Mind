using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 窗口的Control层基类，全为单例模式
/// </summary>
public abstract class BaseControl<T> where T : new()
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

    public abstract void Rigister(BaseView view);
    public abstract void UnRigister(BaseView view);
}