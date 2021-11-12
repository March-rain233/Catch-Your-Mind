using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���ڵ�
/// </summary>
public class RootNode : DecoratorNode
{
    public override bool IsRoot => true;

    protected override NodeStatus OnUpdate(BehaviorTreeRunner runner)
    {
        return Child.Tick(runner);
    }
}
