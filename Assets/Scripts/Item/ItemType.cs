using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 物品信息相关
/// </summary>
namespace Item
{
    /// <summary>
    /// 物品类型
    /// </summary>
    public enum ItemType
    {
        /// <summary>
        /// 所有
        /// </summary>
        All,
        /// <summary>
        /// 装备
        /// </summary>
        Equipment,
        /// <summary>
        /// 武器
        /// </summary>
        Weapon,
        /// <summary>
        /// 消耗品
        /// </summary>
        Consumable,
        /// <summary>
        /// 其他
        /// </summary>
        Other,
    }
}