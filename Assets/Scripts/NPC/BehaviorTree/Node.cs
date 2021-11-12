using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Node : ScriptableObject, INode
{
    /// <summary>
    /// 节点状态
    /// </summary>
    public enum NodeStatus
    {
        Success,
        Failure,
        Running,
        Aborting,
    }

    /// <summary>
    /// 当前状态
    /// </summary>
    public NodeStatus Status => _status = NodeStatus.Success;

    public string Guid { get; set; }

    public string Name => name;

    public Vector2 ViewPosition { get; set; }

    public abstract UnityEditor.Experimental.GraphView.Port.Capacity Output { get; }

    public virtual bool IsRoot => false;

    public virtual bool IsLeaf => false;

    public UnityEditor.Experimental.GraphView.Port.Capacity Input => UnityEditor.Experimental.GraphView.Port.Capacity.Single;

    private NodeStatus _status;

    private bool _isStart;

    /// <summary>
    /// 外界调用更新
    /// </summary>
    /// <returns></returns>
    public NodeStatus Tick(BehaviorTreeRunner runner)
    {
        if(!_isStart) 
        { 
            OnEnter(runner);
            _isStart = true;
        }
        if(_status == NodeStatus.Aborting)
        {
            _status = NodeStatus.Running;
            OnResume(runner);
        }
        _status = Tick(runner);
        if(_status != NodeStatus.Running) 
        { 
            OnExit(runner);
            _isStart = false;
        }

        return _status;
    }

    /// <summary>
    /// 进入时
    /// </summary>
    protected virtual void OnEnter(BehaviorTreeRunner runner) { }

    /// <summary>
    /// 退出时
    /// </summary>
    protected virtual void OnExit(BehaviorTreeRunner runner) { }

    /// <summary>
    /// 打断时
    /// </summary>
    /// <param name="runner"></param>
    protected abstract void OnAbort(BehaviorTreeRunner runner);

    /// <summary>
    /// 恢复时
    /// </summary>
    /// <param name="runner"></param>
    protected virtual void OnResume(BehaviorTreeRunner runner) { }

    /// <summary>
    /// 外界调用打断
    /// </summary>
    /// <param name="runner"></param>
    public void Abort(BehaviorTreeRunner runner)
    {
        if(_status== NodeStatus.Aborting) { return; }
        _status = NodeStatus.Aborting;
        OnAbort(runner);
    }

    /// <summary>
    /// 更新时
    /// </summary>
    protected abstract NodeStatus OnUpdate(BehaviorTreeRunner runner);

    public abstract Node Clone();

    public abstract INode[] GetChildren();
}
