using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ¸ù½Úµã
/// </summary>
public class RootNode : DecoratorNode
{

    protected override NodeStatus OnUpdate(BehaviorTreeRunner runner)
    {
        return Child.Tick(runner);
    }
}
