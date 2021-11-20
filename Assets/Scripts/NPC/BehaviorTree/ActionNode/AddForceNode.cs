using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPC
{

    public class AddForceNode : ActionNode
    {
        public Vector2 Force;

        protected override void OnAbort(BehaviorTreeRunner runner)
        {

        }

        protected override void OnEnter(BehaviorTreeRunner runner)
        {

        }

        protected override void OnExit(BehaviorTreeRunner runner)
        {

        }

        protected override void OnResume(BehaviorTreeRunner runner)
        {

        }

        protected override NodeStatus OnUpdate(BehaviorTreeRunner runner)
        {
            (runner.Variables["Rigidbody"].Object as Rigidbody2D).AddForce(Force);
            return NodeStatus.Success;
        }
    }
}
