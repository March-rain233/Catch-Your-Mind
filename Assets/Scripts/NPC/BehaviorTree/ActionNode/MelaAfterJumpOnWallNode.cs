using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPC
{

    public class MelaAfterJumpOnWallNode : StateNode
    {

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
