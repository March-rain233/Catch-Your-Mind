using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPC
{

    public class MelaIdle : StateNode
    {
        /// <summary>
        /// 预计的停留时间
        /// </summary>
        public float StayTime;

        /// <summary>
        /// 最大停留时间
        /// </summary>
        public float MaxStayTime;

        /// <summary>
        /// 最小停留时间
        /// </summary>
        public float MinStayTime;

        /// <summary>
        /// 进入状态的时间
        /// </summary>
        private float _enterTime;

        protected override void OnEnter(BehaviorTreeRunner runner)
        {
            base.OnEnter(runner);
            _enterTime = Time.time;
            StayTime = Random.Range(MinStayTime, MaxStayTime);
        }

        protected override NodeStatus OnUpdate(BehaviorTreeRunner runner)
        {
            if(Time.time - _enterTime >= StayTime)
            {
                return NodeStatus.Success;
            }
            _rg.velocity = Vector2.zero;
            return NodeStatus.Running;
        }
    }
}
