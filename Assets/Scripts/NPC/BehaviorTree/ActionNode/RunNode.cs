using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPC
{
    public class RunNode : ActionNode
    {
        /// <summary>
        /// 移动倍率
        /// </summary>
        [SerializeField]
        private float _moveScale;

        private Animator _animator;
        private Rigidbody2D _rg;
        private NPC_Model _model;

        protected override void OnAbort(BehaviorTreeRunner runner)
        {
            OnExit(runner);
        }

        protected override void OnEnter(BehaviorTreeRunner runner)
        {
            _animator = runner.Variables["Animator"] as Animator;
            _rg = runner.Variables["Rigidbody"] as Rigidbody2D;
            _model = runner.Variables["Model"] as NPC_Model;
            _animator.SetBool("IsRun", true);
        }

        protected override void OnExit(BehaviorTreeRunner runner)
        {
            _animator.SetBool("IsRun", false);
        }

        protected override void OnResume(BehaviorTreeRunner runner)
        {
            OnEnter(runner);
        }

        protected override NodeStatus OnUpdate(BehaviorTreeRunner runner)
        {
            var pair = GameManager.Instance.ControlManager.KeyDic;
            //左-1 中0 右1
            int horizontal =
                System.Convert.ToInt32(UnityEngine.Input.GetKey(pair[KeyType.Right]))
                - System.Convert.ToInt32(UnityEngine.Input.GetKey(pair[KeyType.Left]));

            //下-1 中0 上1
            bool jump = UnityEngine.Input.GetKeyDown(pair[KeyType.Jump]);
            //左-1 中0 右1

            if (horizontal == 0) { return NodeStatus.Running; }
            if (horizontal == 1)
            {
                _model.FaceDir = 1;
            }
            else
            {
                _model.FaceDir = -1;
            }
            _rg.velocity = new Vector2(horizontal * _moveScale * _model.Speed, 0);
            return NodeStatus.Running;
        }
    }
}
