using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPC {

    public class CheckWallNode : ConditionNode
    {
        public Vector2 Offset;

        public float Lenth;

        public LayerMask Layer;

        protected override NodeStatus OnUpdate(BehaviorTreeRunner runner)
        {
            Vector2 temp = Offset;
            float face = (runner.Variables["Model"].Object as NPC_Model).FaceDir;
            temp.x *= face;
            var hit = RayCast((Vector2)runner.transform.position + temp, face * Vector2.right, Lenth);
            if (hit)
            {
                return NodeStatus.Success;
            }
            else
            {
                return NodeStatus.Failure;
            }
        }

        private RaycastHit2D RayCast(Vector2 ori, Vector2 dir, float dis)
        {
            Physics2D.queriesStartInColliders = false;
            var hit = Physics2D.Raycast(ori, dir, dis, Layer);
            Debug.DrawRay(ori, dir * dis, hit ? Color.red : Color.green);
            Physics2D.queriesStartInColliders = true;
            return hit;
        }
    }
}