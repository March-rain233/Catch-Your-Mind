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
            Debug.DrawLine((Vector2)runner.transform.position - Radius * Vector2.up, (Vector2)runner.transform.position + Radius * Vector2.up, Color.red);
            Debug.DrawLine((Vector2)runner.transform.position - Radius * Vector2.left, (Vector2)runner.transform.position + Radius * Vector2.left, Color.red);
            if (hit == null) { return NodeStatus.Failure; }
            return NodeStatus.Success;
        }
    }
}
