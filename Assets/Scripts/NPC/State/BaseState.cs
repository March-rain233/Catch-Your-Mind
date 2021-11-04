using Item;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPC
{
    /// <summary>
    /// 角色状态基类
    /// </summary>
    /// <remarks>
    /// 请将所有字段设为外界可读以便过渡类可以监视
    /// </remarks>
    public abstract class BaseState : ScriptableObject
    {
        /// <summary>
        /// 逻辑执行函数
        /// </summary>
        /// <returns>下一状态</returns>
        public abstract void OnUpdate(BehaviorStateMachine user);

        /// <summary>
        /// 当进入状态时
        /// </summary>
        public virtual void OnEnter(BehaviorStateMachine user)
        {
            Debug.Log($"进入{this.name}状态");
            user.Animator.SetBool("FaceDirection", user.Model.FaceDir);
        }

        /// <summary>
        /// 当脱离状态时
        /// </summary>
        public virtual void OnExit(BehaviorStateMachine user)
        {
            Debug.Log($"退出{this.name}状态");
        }

        /// <summary>
        /// 当遭到伤害时的计算逻辑
        /// </summary>
        /// <param name="user">承受者</param>
        /// <param name="hurt">攻击值</param>
        /// <param name="status">附加状态</param>
        public virtual void Hurt(BehaviorStateMachine user, float hurt, Status status)
        {
        }
    }
}