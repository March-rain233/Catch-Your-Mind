using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPC
{
    [CreateAssetMenu(fileName = "麦德跳跃状态", menuName = "角色/状态/麦德跳跃")]
    public class MindJump : BaseState
    {
        /// <summary>
        /// 跳跃程度
        /// </summary>
        [SerializeField]
        private float _jumpScale;

        /// <summary>
        /// 移动倍率
        /// </summary>
        [SerializeField]
        private float _moveScale;

        public override void OnEnter(BehaviorStateMachine user)
        {
            base.OnEnter(user);
            user.Model.RigidBody.AddForce(Vector2.up * _jumpScale * user.Model.JumpHeight);
            user.Animator.SetBool("IsJump", true);
        }

        public override void OnExit(BehaviorStateMachine user)
        {
            base.OnExit(user);
            user.Animator.SetBool("IsJump", false);
        }

        public override void OnUpdate(BehaviorStateMachine user)
        {
            var pair = GameSystem.Instance.ControlManager.KeyDic;
            //左-1 中0 右1
            int horizontal =
                System.Convert.ToInt32(Input.GetKey(pair[KeyType.Right]))
                - System.Convert.ToInt32(Input.GetKey(pair[KeyType.Left]));

            //下-1 中0 上1
            bool jump = Input.GetKeyDown(pair[KeyType.Up]);
            //左-1 中0 右1

            if (horizontal == 0) { return; }
            if (horizontal == 1)
            {
                user.Model.FaceDir = true;
            }
            else
            {
                user.Model.FaceDir = false;
            }
            user.Model.RigidBody.velocity = new Vector2(horizontal * _moveScale * user.Model.Speed
                , user.Model.RigidBody.velocity.y);
        }
    }
}
