using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPC
{

    public class HoldNode : MindNode
    {
        protected override void OnEnter(BehaviorTreeRunner runner)
        {
            base.OnEnter(runner);
            _rg.velocity = Vector2.zero;
        }

        protected override NodeStatus OnUpdate(BehaviorTreeRunner runner)
        {
            return NodeStatus.Running;
        }
    }
}
