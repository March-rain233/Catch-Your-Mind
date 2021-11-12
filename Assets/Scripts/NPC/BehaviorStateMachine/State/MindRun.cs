using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPC
{
    [CreateAssetMenu(fileName = "麦德奔跑状态", menuName = "角色/状态/麦德奔跑")]
    public class MindRun : BaseState
    {
        /// <summary>
        /// 移动倍率
        /// </summary>
        [SerializeField]
        private float _moveScale;

        public override void OnEnter(BehaviorStateMachine user)
        {
            base.OnEnter(user);
            user.Animator.SetBool("IsRun", true);
        }

        public override void OnExit(BehaviorStateMachine user)
        {
            base.OnExit(user);
            user.Animator.SetBool("IsRun", false);
        }

        public override void OnUpdate(BehaviorStateMachine user)
        {
            var pair = GameManager.Instance.ControlManager.KeyDic;
            //左-1 中0 右1
            int horizontal =
                System.Convert.ToInt32(UnityEngine.Input.GetKey(pair[KeyType.Right]))
                - System.Convert.ToInt32(UnityEngine.Input.GetKey(pair[KeyType.Left]));

            //下-1 中0 上1
            bool jump = UnityEngine.Input.GetKeyDown(pair[KeyType.Jump]);
            //左-1 中0 右1

            if (horizontal == 0) { return; }
            if (horizontal == 1)
            {
                user.Model.FaceDir = 1;
            }
            else
            {
                user.Model.FaceDir = -1;
            }
            user.Model.RigidBody.velocity = new Vector2(horizontal * _moveScale * user.Model.Speed, 0);
        }
    }
}
