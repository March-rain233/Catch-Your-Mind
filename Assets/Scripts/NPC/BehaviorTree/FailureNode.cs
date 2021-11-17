using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPC
{
    public class FailureNode : DecoratorNode
    {
        /// <summary>
        /// ÓÀ¼ÙÆ÷
        /// </summary>
        /// <param name="runner"></param>
        /// <returns></returns>
        protected override NodeStatus OnUpdate(BehaviorTreeRunner runner)
        {
            Child.Tick(runner);
            return NodeStatus.Failure;
        }
    }
}