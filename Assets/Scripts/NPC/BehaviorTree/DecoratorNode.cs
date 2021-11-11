using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 修饰节点
/// </summary>
public abstract class DecoratorNode : Node
{
    /// <summary>
    /// 子节点
    /// </summary>
    public Node Child;

    protected override void OnAbort(BehaviorTreeRunner runner)
    {
        if(Child.Status == NodeStatus.Running) { Child.Abort(runner); }
    }

    public override Node Clone()
    {
        var node = Instantiate(this);
        Child = Child.Clone();
        return node;
    }
}
