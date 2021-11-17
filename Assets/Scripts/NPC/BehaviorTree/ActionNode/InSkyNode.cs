using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPC
{
    public class InSkyNode : MindNode
    {
        [SerializeField]
        private float _moveScale;

        protected override void OnEnter(BehaviorTreeRunner runner)
        {
            _animator = runner.Variables["Animator"].Object as Animator;
            _rg = runner.Variables["Rigidbody"].Object as Rigidbody2D;
            _model = runner.Variables["Model"].Object as NPC_Model;
        }

        protected override NodeStatus OnUpdate(BehaviorTreeRunner runner)
        {
            var pair = GameManager.Instance.ControlManager.KeyDic;
            //��-1 ��0 ��1
            int horizontal =
                System.Convert.ToInt32(UnityEngine.Input.GetKey(pair[KeyType.Right]))
                - System.Convert.ToInt32(UnityEngine.Input.GetKey(pair[KeyType.Left]));

            //��-1 ��0 ��1
            bool jump = UnityEngine.Input.GetKeyDown(pair[KeyType.Jump]);
            //��-1 ��0 ��1

            if (horizontal == 0) { return NodeStatus.Running; }
            if (horizontal == 1)
            {
                _model.FaceDir = 1;
            }
            else
            {
                _model.FaceDir = -1;
            }
            _rg.AddForce(new Vector2(horizontal * _moveScale * _model.Speed, 0));
            return NodeStatus.Running;
        }
    }
}