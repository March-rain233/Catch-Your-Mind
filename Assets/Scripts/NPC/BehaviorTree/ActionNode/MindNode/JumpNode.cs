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

        private float _lastTime = -1;

        private bool _hold = false;

        protected override void OnExit(BehaviorTreeRunner runner)
        {
            _lastTime = -1;
            _hold = false;
        }

        protected override void OnEnter(BehaviorTreeRunner runner)
        {
            _animator = runner.Variables["Animator"].Object as Animator;
            _rg = runner.Variables["Rigidbody"].Object as Rigidbody2D;
            _model = runner.Variables["Model"].Object as NPC_Model;
        }

        protected override NodeStatus OnUpdate(BehaviorTreeRunner runner)
        {
            if(_hold && (Time.time - _lastTime >= _holdingTime || UnityEngine.Input.GetKeyUp(GameManager.Instance.ControlManager.KeyDic[KeyType.Jump])))
            {
                float time = Time.time - _lastTime;
                float rate = Mathf.Clamp(time, 0, _holdingTime);
                rate = rate / _holdingTime;
                rate = Mathf.Lerp(_minJumpScale, _maxJumpScale, rate);
                //_rg.velocity = Vector2.zero;

                var p = _rg.transform.position;
                p.y += 0.1f;
                //_rg.transform.position = p;
                _rg.AddForce(Vector2.up * rate * _model.JumpHeight);
                return NodeStatus.Success;
                _lastTime = -1f;
                _hold = false;
            }

            if (UnityEngine.Input.GetKey(GameManager.Instance.ControlManager.KeyDic[KeyType.Jump]))
            {
                if(!_hold)
                {
                    _lastTime = Time.time;
                    _hold = true;
                }
                return NodeStatus.Running;
            }

            return NodeStatus.Failure;
        }
    }
}
