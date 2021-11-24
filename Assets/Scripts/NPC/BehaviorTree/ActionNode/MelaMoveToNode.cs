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
        private Vector2 _target;

        /// <summary>
        /// 移动范围的左下角
        /// </summary>
        public Vector2 MinPos;
        /// <summary>
        /// 移动范围的右上角
        /// </summary>
        public Vector2 MaxPos;

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
