using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 视图的基类，作为View层管控窗口的显示
/// </summary>
public abstract class BaseView : MonoBehaviour
{
    /// <summary>
    /// 视图动画器
    /// </summary>
    [SerializeField]
    protected Animator _animator;

    /// <summary>
    /// UI是否启用
    /// </summary>
    public bool IsEnable
    {
        get
        {
            return _isEnable;
        }
        protected set
        {
            _isEnable = value;
        }
    }
    private bool _isEnable;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        Init();
    }

    private void Start()
    {
        OnEnter();
    }

    /// <summary>
    /// 设置UI的启用属性
    /// </summary>
    /// <param name="arg"></param>
    public void SetIsEnable(bool arg)
    {
        IsEnable = arg;
        if (arg)
        {
            OnResume();
        }
        else
        {
            OnPause();
        }
    }

    /// <summary>
    /// UI弹出消息
    /// </summary>
    protected internal abstract void OnEnter();

    /// <summary>
    /// UI暂停消息
    /// </summary>
    protected abstract void OnPause();

    /// <summary>
    /// UI恢复消息
    /// </summary>
    protected abstract void OnResume();

    /// <summary>
    /// UI关闭消息
    /// </summary>
    protected abstract void OnExit();

    /// <summary>
    /// UI更新方法
    /// </summary>
    /// <remarks>
    /// 如果UI启用，将在每一帧更新
    /// </remarks>
    protected internal virtual void OnUpdate()
    {

    }

    /// <summary>
    /// 由外界调用来管理生命周期（退出ui）
    /// </summary>
    /// <param name="resourceRecoveryHandler">
    /// 资源回收处理委托
    /// </param>
    /// <remarks>
    /// 重写时要注意需要在最后调用委托通知管理器回收
    /// </remarks>
    protected internal virtual void SendExit(System.Action<BaseView> resourceRecoveryHandler)
    {
        if (_animator != null)
        {
            StartCoroutine(ObserveAnimProgress("Exit", 1, () =>
            {
                resourceRecoveryHandler.Invoke(this);
            }));
        }
        OnExit();
        if (_animator == null)
        {
            resourceRecoveryHandler.Invoke(this);
        }
    }

    /// <summary>
    /// 监视动画状态的运行进度
    /// </summary>
    /// <param name="tag">状态的标签</param>
    /// <param name="time">目标进度</param>
    /// <param name="callback">回调函数</param>
    /// <returns></returns>
    public IEnumerator ObserveAnimProgress(string tag, float time, System.Action callback)
    {
        //获取目标状态，注册监视
        AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
        while (!stateInfo.IsTag(tag))
        {
            Debug.Log($"未进入{tag}动画");
            yield return 0;
            stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
        }

        //监视动画运行状态
        while (stateInfo.normalizedTime < time)
        {
            //Debug.Log($"动画{stateInfo.fullPathHash} 运行进程：{stateInfo.normalizedTime}");
            yield return 0;
            stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
        }

        //调用回调函数通知
        callback?.Invoke();
    }

    private void Update()
    {
        if (!IsEnable)
        {
            return;
        }

        OnUpdate();
    }

    /// <summary>
    /// 分发model传来的数据
    /// </summary>
    /// <param name="name">数据名</param>
    /// <param name="value">数据</param>
    public abstract void NotifyHandler(string name, object value);

    /// <summary>
    /// 初始化UI视图
    /// </summary>
    protected internal abstract void Init();
}
