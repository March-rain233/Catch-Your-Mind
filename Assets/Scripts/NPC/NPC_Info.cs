using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Sirenix.OdinInspector;

namespace Save
{
    /// <summary>
    /// 角色存档
    /// </summary>
    [CreateAssetMenu(fileName = "角色信息", menuName = "角色信息")]
    [Serializable]
    public class NPC_Info : SerializedScriptableObject
    {
        /// <summary>
        /// 角色种族的名称
        /// </summary>
        public string Breed;

        /// <summary>
        /// 角色装备
        /// </summary>
        public EquipSave Equip;

        /// <summary>
        /// 行为模式的名称
        /// </summary>
        public string Behavior;

        /// <summary>
        /// 预制体名称
        /// </summary>
        public string Prefabs;

        /// <summary>
        /// 角色血量
        /// </summary>
        public float Blood;

        /// <summary>
        /// 角色能量
        /// </summary>
        public float Energy;
    }
}
