using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPC
{
    /// <summary>
    /// �������
    /// </summary>
    [CreateAssetMenu(fileName = "����ƶ�״̬", menuName = "��ɫ/״̬/����ƶ�")]
    public class MindWalk : BaseState
    {
        /// <summary>
        /// �ƶ�����
        /// </summary>
        [SerializeField]
        private float _moveScale;

        public override void OnEnter(BehaviorStateMachine user)
        {
            base.OnEnter(user);
            user.Animator.SetBool("Walk", true);
        }

        public override void OnExit(BehaviorStateMachine user)
        {
            base.OnExit(user);
            user.Animator.SetBool("Walk", false);
        }

        public override void OnUpdate(BehaviorStateMachine user)
        {
            var pair = GameSystem.Instance.ControlManager.KeyDic;
            //��-1 ��0 ��1
            int horizontal =
                System.Convert.ToInt32(Input.GetKey(pair[KeyType.Right]))
                - System.Convert.ToInt32(Input.GetKey(pair[KeyType.Left]));

            //��-1 ��0 ��1
            bool jump = Input.GetKeyDown(pair[KeyType.Up]);
            //��-1 ��0 ��1

            if (horizontal == 0) { return; }
            if(horizontal == 1)
            {
                user.Model.FaceDir = true;
            }
            else
            {
                user.Model.FaceDir = false;
            }
            user.Model.RigidBody.velocity = new Vector2(horizontal * _moveScale * user.Model.Speed, 0);
        }
    }
}
