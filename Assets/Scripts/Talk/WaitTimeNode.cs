using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogue {

    public class WaitTimeNode : ActionNode
    {
        public float WaitTime;

        private float _enterTime;

        protected override void OnEnter(DialogueTree tree)
        {
            base.OnEnter(tree);
            _enterTime = Time.time;
        }

        protected override NodeStatus OnUpdate(DialogueTree tree)
        {
            if (Time.time - _enterTime >= WaitTime)
            {
                return NodeStatus.Success;
            }
            return NodeStatus.Running;
        }
    }
}
