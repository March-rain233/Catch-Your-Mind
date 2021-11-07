using UnityEngine;

namespace NPC
{
    [CreateAssetMenu(fileName = "麦德上升状态", menuName = "角色/状态/麦德上升")]
    public class MindRise : BaseState
    {
        /// <summary>
        /// 移动倍率
        /// </summary>
        [SerializeField]
        private float _moveScale;

        public override void OnEnter(BehaviorStateMachine user)
        {
            base.OnEnter(user);
            user.Animator.SetBool("IsRise", true);
        }

        public override void OnExit(BehaviorStateMachine user)
        {
            base.OnExit(user);
            user.Animator.SetBool("IsRise", false);
        }

        public override void OnUpdate(BehaviorStateMachine user)
        {
            var pair = GameManager.Instance.ControlManager.KeyDic;
            //左-1 中0 右1
            int horizontal =
                System.Convert.ToInt32(Input.GetKey(pair[KeyType.Right]))
                - System.Convert.ToInt32(Input.GetKey(pair[KeyType.Left]));

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
