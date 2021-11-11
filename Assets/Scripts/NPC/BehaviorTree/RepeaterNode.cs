using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeaterNode : DecoratorNode
{
    /// <summary>
    /// 运行次数
    /// </summary>
    public int Times;

    /// <summary>
    /// 累计次数
    /// </summary>
    private int _add;

    /// <summary>
    /// 是否无限运行
    /// </summary>
    public bool IsForever;

    protected override void OnEnter(BehaviorTreeRunner runner)
    {
        base.OnEnter(runner);
        _add = 0;
    }

    protected override NodeStatus OnUpdate(BehaviorTreeRunner runner)
    {
        Child.Tick(runner);
        ++_add;
        if (IsForever || _add < Times)
        {
            return NodeStatus.Running;
        }
        return NodeStatus.Success;
    }
}
