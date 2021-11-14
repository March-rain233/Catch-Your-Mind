using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPC
{
    /// <summary>
    /// ¸ù½Úµã
    /// </summary>
    public class RootNode : DecoratorNode
    {
        public override bool IsRoot => true;

        protected override NodeStatus OnUpdate(BehaviorTreeRunner runner)
        {
            return Child.Tick(runner);
        }
    }
}
