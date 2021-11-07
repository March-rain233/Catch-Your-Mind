using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Sirenix.OdinInspector;

/// <summary>
/// 功能面板管理
/// </summary>
/// <remarks>
/// 底层是ui栈，负责管理功能面板的UI周期
/// </remarks>
public class PanelManager
{
    /// <summary>
    /// 根面板
    /// </summary>
    private Transform _canvasTransform;
    public Transform CanvasTransform
    {
        get
        {
            if (_canvasTransform == null)
            {
                _canvasTransform = GameObject.Find("UIroot").transform;
                GameObject.DontDestroyOnLoad(_canvasTransform);
            }
            return _canvasTransform;
        }
    }

    /// <summary>
    /// 窗口栈
    /// </summary>
    [SerializeField]
    private Stack<BasePanel> _windowsStack;

    /// <summary>
    /// 最顶层窗口的窗口类型
    /// </summary>
    public PanelType TopType
    {
        get
        {
            return Peek().Type;
        }
    }

    /// <summary>
    /// 当前使用的工厂
    /// </summary>
    private PanelFactory _factory = new PanelFactory();

    /// <summary>
    /// 生成窗口
    /// </summary>
    /// <param name="windowType">窗口类型</param>
    /// <param name="window">窗口实例</param>
    /// <returns>是否存在该窗口的预制体</returns>
    private bool TryGetWindow(PanelType windowType, out BasePanel window)
    {
        bool val = _factory.TryGetWindow(windowType, out window);
        if (val)
        {
            window.transform.SetParent(CanvasTransform);
            var t = window.GetComponent<RectTransform>();
            t.offsetMax = t.offsetMin = Vector2.zero;
        }
        return val;
    }

    /// <summary>
    /// 将UI压入栈
    /// </summary>
    /// <param name="windowType">窗口类型</param>
    public void Push(PanelType windowType)
    {
        //判断栈是否生成，并检查栈顶UI是否存在
        if (_windowsStack == null)
        {
            _windowsStack = new Stack<BasePanel>();
        }
        UpdateStack();

        //压栈，并停止原先的栈顶
        if (_windowsStack.Count > 0)
        {
            _windowsStack.Peek().SetIsEnable(false);
        }
        BasePanel window;
        TryGetWindow(windowType, out window);
        window.OnEnter();
        _windowsStack.Push(window);
    }

    /// <summary>
    /// 将栈顶UI出栈
    /// </summary>
    public void Pop()
    {
        //判断栈是否生成，并检查栈顶UI是否存在
        if (_windowsStack == null)
        {
            _windowsStack = new Stack<BasePanel>();
        }
        UpdateStack();
        if (_windowsStack.Count <= 0)
        {
            return;
        }

        //出栈
        var temp = _windowsStack.Pop();
        if (temp == null) return;
        temp.SendExit((BaseView view) =>
        { _factory.PushToThePool(view as BasePanel); });
        if (_windowsStack.Count > 0)
        {
            _windowsStack.Peek().SetIsEnable(true);
        }
    }

    /// <summary>
    /// 获取最顶部的UI
    /// </summary>
    /// <returns>最顶部UI</returns>
    public BasePanel Peek()
    {
        if (_windowsStack.Count == 0)
        {
            return null;
        }
        return _windowsStack.Peek();
    }

    /// <summary>
    /// 移除窗口
    /// </summary>
    public void Remove(PanelType type)
    {
        if (!Contain(type))
        {
            Debug.LogWarning($"当前栈内不存在{type}");
            return;
        }

        if (type == TopType)
        {
            Pop();
            return;
        }

        Stack<BasePanel> above = new Stack<BasePanel>();
        int len = _windowsStack.Count;
        for (int ctr = 0; ctr < len; ++ctr)
        {
            //查找指定窗口并关闭，把其他窗口转入临时的栈
            if (TopType == type)
            {
                var window = _windowsStack.Pop();
                window.SendExit((BaseView view) =>
                { _factory.PushToThePool(view as BasePanel); });
                break;
            }
            above.Push(_windowsStack.Pop());
        }

        //将临时栈内的窗口导回
        while (above.Count > 0)
        {
            _windowsStack.Push(above.Pop());
        }
    }

    /// <summary>
    /// 将窗口置顶
    /// </summary>
    public void SetTop(PanelType type)
    {
        if (!Contain(type))
        {
            Debug.LogWarning($"当前栈内不存在{type}");
            return;
        }

        //当当前窗口已为最顶则直接返回
        if (type == TopType)
        {
            return;
        }

        Stack<BasePanel> above = new Stack<BasePanel>();
        int len = _windowsStack.Count;
        BasePanel top = null;
        for (int ctr = 0; ctr < len; ++ctr)
        {
            //查找指定窗口，把其他窗口转入临时的栈
            if (TopType == type)
            {
                top = _windowsStack.Pop();
                break;
            }
            above.Push(_windowsStack.Pop());
        }

        //将临时栈内的窗口导回，并关闭原先活动窗口，将目标窗口激活
        while (above.Count > 0)
        {
            _windowsStack.Push(above.Pop());
        }
        _windowsStack.Peek().SetIsEnable(false);
        _windowsStack.Push(top);
        top.SetIsEnable(true);
    }

    /// <summary>
    /// 判断是否已存在该窗口
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public bool Contain(PanelType type)
    {
        foreach (var i in _windowsStack)
        {
            if (i.Type == type)
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// 清除已经不存在的栈顶UI
    /// </summary>
    private void UpdateStack()
    {
        while (_windowsStack.Count != 0)
        {
            if (_windowsStack.Peek() == null)
            {
                _windowsStack.Pop();
            }
            else
            {
                break;
            }
        }
    }
}
