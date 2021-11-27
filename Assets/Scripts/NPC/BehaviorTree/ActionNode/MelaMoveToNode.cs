using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPC {

    /// <summary>
    /// 梅拉随机移动
    /// </summary>
    public class MelaMoveToNode : StateNode
    {
        [SerializeField]
        private Queue<Vector2> _targets = new Queue<Vector2>();

        /// <summary>
        /// 移动范围的左下角
        /// </summary>
        public Vector2 MinPos;
        /// <summary>
        /// 移动范围的右上角
        /// </summary>
        public Vector2 MaxPos;

        public int MaxCount;
        public int MinCount = 1;

        /// <summary>
        /// 移动速度倍率
        /// </summary>
        public float VelocityRate;

        /// <summary>
        /// 距离判定
        /// </summary>
        /// <remarks>
        /// 当与目标点相距小于该值时判定为达到目标点
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
