using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPC
{

    public class JumpNode : MindNode
    {

        /// <summary>
        /// �����Ծ����
        /// </summary>
        [SerializeField]
        private float _maxJumpScale;

        /// <summary>
        /// ��С��Ծ����
        /// </summary>
        [SerializeField]
        private float _minJumpScale;

        /// <summary>
        /// ���������Чʱ��
        /// </summary>
        [SerializeField]
        private float _holdingTime;

        private float _lastTime = -1;

        protected override void OnExit(BehaviorTreeRunner runner)
        {
            _lastTime = -1;
        }

        protected override void OnEnter(BehaviorTreeRunner runner)
        {
            _animator = runner.Variables["Animator"].Object as Animator;
            _rg = runner.Variables["Rigidbody"].Object as Rigidbody2D;
            _model = runner.Variables["Model"].Object as NPC_Model;
        }

        protected override NodeStatus OnUpdate(BehaviorTreeRunner runner)
        {
            if(_lastTime != -1 && (Time.time - _lastTime >= _holdingTime || UnityEngine.Input.GetKeyUp(GameManager.Instance.ControlManager.KeyDic[KeyType.Jump])))
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
            }

            if (UnityEngine.Input.GetKey(GameManager.Instance.ControlManager.KeyDic[KeyType.Jump]))
            {
                if(_lastTime == -1)
                {
                    _lastTime = Time.time;
                }
                return NodeStatus.Running;
            }

            return NodeStatus.Failure;
        }
    }
}
