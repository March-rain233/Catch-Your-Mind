using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPC
{

    public class MelaIdle : StateNode
    {
        /// <summary>
        /// Ԥ�Ƶ�ͣ��ʱ��
        /// </summary>
        public float StayTime;

        /// <summary>
        /// ���ͣ��ʱ��
        /// </summary>
        public float MaxStayTime;

        /// <summary>
        /// ��Сͣ��ʱ��
        /// </summary>
        public float MinStayTime;

        /// <summary>
        /// ����״̬��ʱ��
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
