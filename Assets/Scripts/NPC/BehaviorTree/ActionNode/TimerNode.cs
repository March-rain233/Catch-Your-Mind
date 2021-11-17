using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPC
{
    /// <summary>
    /// 计时器
    /// </summary>
    public class TimerNode : ActionNode
    {
        /// <summary>
        /// 间隔时间
        /// </summary>
        [SerializeField]
        private float _intervalTime;

        private float _enterTime;

        protected override void OnAbort(BehaviorTreeRunner runner)
        {
            
        }

        protected override void OnEnter(BehaviorTreeRunner runner)
        {
            _enterTime = Time.time;
        }

        protected override void OnExit(BehaviorTreeRunner runner)
        {
            
        }

        protected override void OnResume(BehaviorTreeRunner runner)
        {
            
        }

        protected override NodeStatus OnUpdate(BehaviorTreeRunner runner)
        {
            if(Time.time - _enterTime >= _intervalTime) 
            {
                return NodeStatus.Success;
            }
            return NodeStatus.Running;
        }
    }
}