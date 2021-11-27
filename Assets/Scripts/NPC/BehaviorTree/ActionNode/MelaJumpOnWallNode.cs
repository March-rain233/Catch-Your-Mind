using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPC {

    public class MelaJumpOnWallNode : StateNode
    {
        protected override void OnEnter(BehaviorTreeRunner runner)
        {
            base.OnEnter(runner);
            (runner.Variables["Trail"].Object as TrailRenderer).emitting = false;
            _rg.velocity = Vector2.zero;
            (runner.Variables["Collider"].Object as Collider2D).enabled = true;
            var t = runner.Variables["IsCatchable"];
            t.Boolean = true;
            runner.Variables["IsCatchable"] = t;
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
