using Item;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPC
{
    public interface IHurt
    {
        /// <summary>
        /// 请通过enemy调用传入伤害与伤害类别(默认为普通攻击)
        /// 再通过角色本身计算实际伤害
        /// 最后在血量上扣除
        /// </summary>
        void Hurt(float hurt, Status status = Status.Common);
    }
}
