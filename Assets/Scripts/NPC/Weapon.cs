using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NPC;
using System;

namespace Item
{
    /// <summary>
    /// 武器类
    /// </summary>
    public abstract class Weapon : BaseItem
    {

        public override ItemType Type
        {
            get
            {
                return ItemType.Weapon;
            }
        }

        /// <summary>
        /// 攻击方法
        /// </summary>
        /// <param name="user">攻击者</param>
        /// <param name="callBack">攻击结束后的回调函数</param>
        public abstract void Attack(BehaviorStateMachine user, Action callBack);

        /// <summary>
        /// 打断攻击
        /// </summary>
        public abstract void Interrupt(BehaviorStateMachine user);

        /// <summary>
        /// 检测是否可以攻击到
        /// </summary>
        /// <param name="user">使用者</param>
        /// <param name="target">目标</param>
        /// <returns></returns>
        public abstract bool Check(BehaviorStateMachine user, BehaviorStateMachine target);
    }
}
