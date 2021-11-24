using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPC {

    /// <summary>
    /// ÷������ƶ�
    /// </summary>
    public class MelaMoveToNode : StateNode
    {
        [SerializeField]
        private Vector2 _target;

        /// <summary>
        /// �ƶ���Χ�����½�
        /// </summary>
        public Vector2 MinPos;
        /// <summary>
        /// �ƶ���Χ�����Ͻ�
        /// </summary>
        public Vector2 MaxPos;

        /// <summary>
        /// �ƶ��ٶȱ���
        /// </summary>
        public float VelocityRate;

        /// <summary>
        /// �����ж�
        /// </summary>
        /// <remarks>
        /// ����Ŀ������С�ڸ�ֵʱ�ж�Ϊ�ﵽĿ���
        /// </remarks>
        public float MinDistance;

        protected override void OnEnter(BehaviorTreeRunner runner)
        {
            base.OnEnter(runner);
            _target = new Vector2(Random.Range(MinPos.x, MaxPos.x), Random.Range(MinPos.y, MaxPos.y));
        }

        protected override NodeStatus OnUpdate(BehaviorTreeRunner runner)
        {
            var dir = _target - (Vector2)runner.transform.position;
            if(dir.magnitude <= MinDistance)
            {
                return NodeStatus.Success;
            }

            dir = dir.normalized;
            _model.FaceDir = dir.x > 0 ? 1 : dir.x < 0 ? -1 : 0;
            _rg.velocity = dir * VelocityRate * _model.Speed;
            return NodeStatus.Running;
        }
    }
}
