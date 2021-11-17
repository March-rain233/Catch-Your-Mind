using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPC
{

    public class JumpNode : MindNode
    {

        /// <summary>
        /// 最大跳跃倍率
        /// </summary>
        [SerializeField]
        private float _maxJumpScale;

        /// <summary>
        /// 最小跳跃倍率
        /// </summary>
        [SerializeField]
        private float _minJumpScale;

        /// <summary>
        /// 蓄力最大有效时间
        /// </summary>
        [SerializeField]
        private float _holdingTime;

        protected override void OnExit(BehaviorTreeRunner runner)
        {
            
        }

        protected override NodeStatus OnUpdate(BehaviorTreeRunner runner)
        {
            float time = Time.time - runner.Variables["JumpHoldingTime"].Float;
            float rate = Mathf.Clamp(time, 0, _holdingTime);
            rate = rate / _holdingTime;
            rate = Mathf.Lerp(_minJumpScale, _maxJumpScale, rate);

            var p = _rg.transform.position;
            p.y += 0.1f;
            _rg.transform.position = p;
            _rg.AddForce(Vector2.up * rate * _model.JumpHeight);
            return NodeStatus.Success;
        }
    }
}
