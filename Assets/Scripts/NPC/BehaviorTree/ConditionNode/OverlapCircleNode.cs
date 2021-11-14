using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPC
{
    public class OverlapCircleNode : ConditionNode
    {
        public float Radius;

        public LayerMask CheckLayer;

        protected override NodeStatus OnUpdate(BehaviorTreeRunner runner)
        {
            var hit = Physics2D.OverlapCircle(runner.transform.position, Radius, CheckLayer);
            if(hit == null) { return NodeStatus.Failure; }
            return NodeStatus.Success;
        }
    }
}
