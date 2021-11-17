using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPC
{

    public class RushNode : MindNode
    {
        /// <summary>
        /// 冲刺持续时间
        /// </summary>
        [SerializeField]
        private float _holdTime;

        /// <summary>
        /// 冲刺倍率
        /// </summary>
        [SerializeField]
        private float _rushScale;

        private float _enterTime;

        /// <summary>
        /// 冲刺时朝向
        /// </summary>
        private int _face;

        protected override void OnEnter(BehaviorTreeRunner runner)
        {
            base.OnEnter(runner);
            _enterTime = Time.time;
            var pair = GameManager.Instance.ControlManager.KeyDic;
            _face =  System.Convert.ToInt32(UnityEngine.Input.GetKey(pair[KeyType.Right]))
                - System.Convert.ToInt32(UnityEngine.Input.GetKey(pair[KeyType.Left]));
            _model.FaceDir = _face;
        }

        protected override NodeStatus OnUpdate(BehaviorTreeRunner runner)
        {
            if(Time.time - _enterTime < _holdTime)
            {
                _rg.velocity = new Vector2(_face * _rushScale * _model.Speed, 0);
                return NodeStatus.Running;
            }
            return NodeStatus.Success;
        }
    }
}
