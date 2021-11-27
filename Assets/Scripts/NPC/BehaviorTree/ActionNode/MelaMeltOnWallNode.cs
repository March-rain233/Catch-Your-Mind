using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPC
{

    public class MelaMeltOnWallNode : StateNode
    {
        protected override void OnEnter(BehaviorTreeRunner runner)
        {
            base.OnEnter(runner);
            _rg.velocity = Vector2.zero;
        }

        protected override void OnExit(BehaviorTreeRunner runner)
        {
            base.OnExit(runner);
            var t = runner.Variables["IsCatchable"];
            t.Boolean = false;
            runner.Variables["IsCatchable"] = t;
            (runner.Variables["Trail"].Object as TrailRenderer).emitting = true;
            (runner.Variables["Collider"].Object as Collider2D).enabled = false;
        }

        protected override NodeStatus OnUpdate(BehaviorTreeRunner runner)
        {
            var state = _animator.GetCurrentAnimatorStateInfo(0);
            if (state.IsName(_triggerName) && state.normalizedTime >= 1)
            {
                return NodeStatus.Success;
            }
            return NodeStatus.Running;
        }
    }
}
