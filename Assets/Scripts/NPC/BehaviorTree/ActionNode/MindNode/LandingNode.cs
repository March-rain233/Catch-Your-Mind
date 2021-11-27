using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPC
{

    public class LandingNode : MindNode
    {
        bool abort = false;

        protected override void OnExit(BehaviorTreeRunner runner)
        {
            base.OnExit(runner);
            var t = runner.Variables["Flying"];
            t.Boolean = false;
            runner.Variables["Flying"] = t;
            abort = false;
        }

        protected override void OnEnter(BehaviorTreeRunner runner)
        {
            base.OnEnter(runner);
            _rg.velocity = Vector2.zero;
            //_animator.Play(_triggerName);
        }

        protected override void OnResume(BehaviorTreeRunner runner)
        {
            //base.OnResume(runner);
            abort = true;
        }

        protected override NodeStatus OnUpdate(BehaviorTreeRunner runner)
        {
            if(abort) { return NodeStatus.Failure; }

            var state = _animator.GetCurrentAnimatorStateInfo(0);
            if (state.IsName(_triggerName))
            {
                if (state.normalizedTime >= 1)
                {
                    return NodeStatus.Success;
                }
                return NodeStatus.Running;
            }
            return NodeStatus.Running;
        }
    }
}
