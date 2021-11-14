using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPC
{
    /// <summary>
    /// ³É¹¦Æ÷
    /// </summary>
    public class SucceederNode : DecoratorNode
    {
        protected override NodeStatus OnUpdate(BehaviorTreeRunner runner)
        {
            Child.Tick(runner);
            return NodeStatus.Success;
        }
    }
}