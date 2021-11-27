using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPC
{

    public class SetVeloctivNode : ActionNode
    {

        public Vector2 Velocity;

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
            var rg = runner.Variables["Rigidbody"].Object as Rigidbody2D;
            rg.velocity = Velocity;
            return NodeStatus.Success;
        }
    }
}