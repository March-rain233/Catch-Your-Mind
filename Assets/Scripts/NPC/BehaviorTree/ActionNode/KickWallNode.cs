using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPC
{

    public class KickWallNode : MindNode
    {
        public Vector2 Force;

        protected override void OnExit(BehaviorTreeRunner runner)
        {
            
        }

        protected override NodeStatus OnUpdate(BehaviorTreeRunner runner)
        {
            Vector2 temp = Force;
            _model.FaceDir = -_model.FaceDir;
            temp.x *= _model.FaceDir;
            _rg.velocity = temp;
            return NodeStatus.Success;
        }
    }
}
