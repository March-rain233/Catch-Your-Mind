using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPC
{

    public class KickWallNode : MindNode
    {
        public Vector2 Velocity;

        protected override void OnEnter(BehaviorTreeRunner runner)
        {
            base.OnEnter(runner);
            Vector2 temp = Velocity;
            _model.FaceDir = -_model.FaceDir;
            temp.x *= _model.FaceDir;
            _rg.velocity = temp;
        }

        protected override NodeStatus OnUpdate(BehaviorTreeRunner runner)
        {
            if(_animator.GetCurrentAnimatorStateInfo(0).IsName(_triggerName)&&_animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
            {
                return NodeStatus.Success;
            }
            return NodeStatus.Running;
        }
    }
}
