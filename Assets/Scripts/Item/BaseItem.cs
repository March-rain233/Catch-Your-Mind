using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Item
{
    /// <summary>
    /// 普通物品
    /// </summary>
    /// <remarks>
    /// 功能只有查看信息
    /// </remarks>
    [CreateAssetMenu(fileName = "Item", menuName = "新の物品")]
    public class BaseItem : ScriptableObject, IProduct
    {
        /// <summary>
        /// 物品图标
        /// </summary>
        [PreviewField(50), LabelText("图标"), BoxGroup("基础属性")]
        public Sprite Icon;

        /// <summary>
        /// 物品标注ID
        /// </summary>
        [MinValue(0), BoxGroup("基础属性")]
        public int ID;

        /// <summary>
        /// 物品名
        /// </summary>
        [LabelText("物品名"), BoxGroup("基础属性")]
        public string ItemName;

        /// <summary>
        /// 物品类型
        /// </summary>
        [SerializeField, LabelText("物品类型")]
        public virtual ItemType Type
        {
            get
            {
                return ItemType.Other;
            }
        }

        /// <summary>
        /// 物品描述文本
        /// </summary>
        [LabelText("描述")]
        public TextAsset Detail
        {
            get;
            protected set;
        }

        public IProduct Clone()
        {
            return this;
        }
    }
}
