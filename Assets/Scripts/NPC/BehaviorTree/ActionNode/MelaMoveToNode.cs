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
        private Queue<Vector2> _targets = new Queue<Vector2>();

        /// <summary>
        /// �ƶ���Χ�����½�
        /// </summary>
        public Vector2 MinPos;
        /// <summary>
        /// �ƶ���Χ�����Ͻ�
        /// </summary>
        public Vector2 MaxPos;

        public int MaxCount;
        public int MinCount = 1;

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

        private Vector2 RandomTarget()
        {
            return new Vector2(Random.Range(MinPos.x, MaxPos.x), Random.Range(MinPos.y, MaxPos.y));
        }

        protected override void OnEnter(BehaviorTreeRunner runner)
        {
            base.OnEnter(runner);
            _targets.Clear();
            int count = Random.Range(MinCount, MaxCount + 1);
            while (--count > 0)
            {
                _targets.Enqueue(RandomTarget());
            }
        }

        protected override NodeStatus OnUpdate(BehaviorTreeRunner runner)
        {
            if (_targets.Count <= 0)
            {
                return NodeStatus.Success;
            }
            var dir = _targets.Peek() - (Vector2)runner.transform.position;
            if(dir.magnitude <= MinDistance)
            {
                _targets.Dequeue();
            }

            dir = dir.normalized;
            _model.FaceDir = dir.x > 0 ? 1 : dir.x < 0 ? -1 : 0;
            _rg.velocity = dir * VelocityRate * _model.Speed;
            return NodeStatus.Running;
        }
    }
}
