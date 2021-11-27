using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPC
{

    public class KeyHoldingNode : ConditionNode
    {
        public KeyType KeyType;
        public float TargetTime;
        private float _lastTime = -1;
        private bool _firstRun = true;

        protected override NodeStatus OnUpdate(BehaviorTreeRunner runner)
        {
            if(_lastTime == -1) { _lastTime = Time.time; }
            if(Time.time - _lastTime >= TargetTime || _firstRun)
            {
                _firstRun = false;
                _lastTime = -1;
                return NodeStatus.Success;
            }
            return NodeStatus.Failure;
        }
    }
}
