using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPC
{
    [CreateAssetMenu(fileName = "麦德蓄力状态", menuName = "角色/状态/麦德蓄力")]
    public class MindHold : BaseState
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

        public override void OnEnter(BehaviorStateMachine user)
        {
            base.OnEnter(user);
            user.Animator.SetBool("IsHold", true);
            user.Model.RigidBody.velocity = new Vector2(0, user.Model.RigidBody.velocity.y);
        }

        public override void OnExit(BehaviorStateMachine user)
        {
            base.OnExit(user);
            user.Animator.SetBool("IsHold", false);

            float rate = Mathf.Clamp(user.RunTime, 0, _holdingTime);
            rate = rate / _holdingTime;
            rate = Mathf.Lerp(_minJumpScale, _maxJumpScale, rate);

            user.Model.RigidBody.AddForce(Vector2.up * rate * user.Model.JumpHeight);
        }

        public override void OnUpdate(BehaviorStateMachine user)
        {
            
        }
    }
}
