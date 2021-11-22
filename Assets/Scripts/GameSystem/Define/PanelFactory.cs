using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 负责生成Panel
/// </summary>
public class PanelFactory
{
    public PanelFactory()
    {
        GameObject.DontDestroyOnLoad(Root);
    }

    private Transform _root;
    /// <summary>
    /// 对象池根地址
    /// </summary>
    public Transform Root
    {
        get
        {
            if (_root == null)
            {
                _root = GameObject.Find("UIroot").transform;
            }
            return _root;
        }
    }

    /// <summary>
    /// 窗口池
    /// </summary>
    private Dictionary<PanelType, BasePanel> _pool = new Dictionary<PanelType, BasePanel>();

    /// <summary>
    /// 生成窗口
    /// </summary>
    /// <param name="windowType">窗口类型</param>
    /// <param name="window">窗口实例</param>
    /// <returns>是否存在该窗口的预制体</returns>
    /// <remarks>
    /// 该方法负责处理窗口的初始化
    /// </remarks>
    public bool TryGetWindow(PanelType windowType, out BasePanel window)
    {
        //如果对象池已有窗口，则直接提取
        if (_pool.TryGetValue(windowType, out window))
        {
            window.Init();
            return true;
        }
        else
        {
            //获取预制体并生成
            var temp = GameObject.Instantiate(
                Resources.Load<GameObject>(GameManager.Instance.
                GameConfig.PathConfig.Paths[ObjectType.UI].
                PathDic[windowType.ToString()]));
            temp.name = temp.name.Replace("(Clone)", "");
            window = temp.GetComponent<BasePanel>();
        }

        window.Init();
        _pool.Add(windowType, window);
        return true;
    }

    /// <summary>
    /// 把销毁的窗口放入对象池
    /// </summary>
    /// <param name="view"></param>
    public void PushToThePool(BasePanel view)
    {
        _pool[view.Type] = view;
        view.transform.parent = Root;
    }
}
