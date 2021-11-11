using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeaterNode : DecoratorNode
{
    /// <summary>
    /// ���д���
    /// </summary>
    public int Times;

    /// <summary>
    /// �ۼƴ���
    /// </summary>
    private int _add;

    /// <summary>
    /// �Ƿ���������
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
