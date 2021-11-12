using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPC
{
    [CreateAssetMenu(fileName = "麦德下落状态", menuName = "角色/状态/麦德下落")]
    public class MindFall : BaseState
    {
        /// <summary>
        /// 移动倍率
        /// </summary>
        [SerializeField]
        private float _moveScale;

        public override void OnEnter(BehaviorStateMachine user)
        {
            base.OnEnter(user);
            user.Animator.SetBool("IsFall", true);
        }

        public override void OnExit(BehaviorStateMachine user)
        {
            base.OnExit(user);
            user.Animator.SetBool("IsFall", false);
        }

        public override void OnUpdate(BehaviorStateMachine user)
        {
            var pair = GameManager.Instance.ControlManager.KeyDic;
            //左-1 中0 右1
            int horizontal =
                System.Convert.ToInt32(UnityEngine.Input.GetKey(pair[KeyType.Right]))
                - System.Convert.ToInt32(UnityEngine.Input.GetKey(pair[KeyType.Left]));

            if (horizontal == 0) { return; }
            if (horizontal == 1)
            {
                user.Model.FaceDir = 1;
            }
            else
            {
                user.Model.FaceDir = -1;
            }
            user.Model.RigidBody.velocity = new Vector2(horizontal * _moveScale * user.Model.Speed
                , user.Model.RigidBody.velocity.y);
        }
    }
}
