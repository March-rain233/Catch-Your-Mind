using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace NPC
{
    public class MindIdleNode : MindNode
    {
        protected override void OnEnter(BehaviorTreeRunner runner)
        {
            base.OnEnter(runner);
            _rg.velocity = Vector2.zero;
        }

        protected override NodeStatus OnUpdate(BehaviorTreeRunner runner)
        {
            _rg.velocity = new Vector2(0, _rg.velocity.y);
            return NodeStatus.Running;
        }
    }
}
