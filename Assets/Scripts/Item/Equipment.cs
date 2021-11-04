using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Item
{
    /// <summary>
    /// 装备
    /// </summary>
    [System.Serializable]
    [CreateAssetMenu(fileName = "EquipMent", menuName = "新の装备")]
    public class Equipment : BaseItem
    {
        /// <summary>
        /// 物品攻击数值
        /// </summary>
        public float Attack
        {
            get;
            protected set;
        }

        /// <summary>
        /// 攻击倍率
        /// </summary>
        public float AttackRate
        {
            get;
            protected set;
        }

        /// <summary>
        /// 物品防御数值
        /// </summary>
        public float Defense
        {
            get;
            protected set;
        }

        public float DefenseRate
        {
            get;
            protected set;
        }

        /// <summary>
        /// 物品血量数值
        /// </summary>
        public float Blood
        {
            get;
            protected set;
        }

        public float BloodRate
        {
            get;
            protected set;
        }

        /// <summary>
        /// 速度基础加成
        /// </summary>
        public float Speed
        {
            get;
            protected set;
        }

        public float SpeedRate
        {
            get;
            protected set;
        }
    }
}
