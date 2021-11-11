using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPC
{
    [CreateAssetMenu(fileName = "按键条件", menuName = "角色/条件/按键条件")]
    public class KeyCondition : BaseCondition
    {
        /// <summary>
        /// 按压类型
        /// </summary>
        [System.Serializable]
        private enum PressType
        {
            Down,
            Pressing,
            Up
        }

        /// <summary>
        /// 检测的按键类型
        /// </summary>
        [SerializeField]
        private KeyType _keyType;

        /// <summary>
        /// 检测的按压类型
        /// </summary>
        [SerializeField]
        private PressType _pressType;

        public override bool Reason(BehaviorStateMachine user)
        {
            switch (_pressType)
            {
                case PressType.Down:
                    return Input.GetKeyDown(GameManager.Instance.ControlManager.KeyDic[_keyType]);
                case PressType.Pressing:
                    return Input.GetKey(GameManager.Instance.ControlManager.KeyDic[_keyType]);
                case PressType.Up:
                    return Input.GetKeyUp(GameManager.Instance.ControlManager.KeyDic[_keyType]);
                default:
                    return true;
            }
        }
    }
}
