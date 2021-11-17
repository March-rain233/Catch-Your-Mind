using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPC
{
    /// <summary>
    /// 检查速度是否超出指定速度
    /// </summary>
    public class VelocityCheckNode : ConditionNode
    {
        /// <summary>
        /// 检测的标准速度
        /// </summary>
        [SerializeField]
        private Vector2 _maxVelocity;

        /// <summary>
        /// 忽略X轴
        /// </summary>
        [SerializeField]
        private bool _ignoreX;
        /// <summary>
        /// 忽略Y轴
        /// </summary>
        [SerializeField]
        private bool _ignoreY;

        protected override NodeStatus OnUpdate(BehaviorTreeRunner runner)
        {
            var rg = runner.Variables["Rigidbody"].Object as Rigidbody2D;
            var uv = rg.velocity;
            var mv = _maxVelocity;
            if (!_ignoreY && mv.y < 0)
            {
                mv.y = -mv.y;
                uv.y = -uv.y;
            }
            if (!_ignoreX && mv.x < 0)
            {
                mv.x = -mv.x;
                uv.x = -uv.x;
            }

            if (Invert ^ ((_ignoreY || uv.y <= mv.y) && (_ignoreX || uv.x <= mv.x))) { return NodeStatus.Failure; }
            return NodeStatus.Success;
        }
    }
}
