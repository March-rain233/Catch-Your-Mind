using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPC
{
    /// <summary>
    /// 检测按键
    /// </summary>
    public class KeyNode : ConditionNode
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

        protected override NodeStatus OnUpdate(BehaviorTreeRunner runner)
        {
            switch (_pressType)
            {
                case PressType.Down:
                    if (UnityEngine.Input.GetKeyDown(GameManager.Instance.ControlManager.KeyDic[_keyType])) { return NodeStatus.Success; }
                    return NodeStatus.Failure;
                case PressType.Pressing:
                    if (UnityEngine.Input.GetKey(GameManager.Instance.ControlManager.KeyDic[_keyType])) { return NodeStatus.Success; }
                    return NodeStatus.Failure;
                case PressType.Up:
                    if (UnityEngine.Input.GetKeyUp(GameManager.Instance.ControlManager.KeyDic[_keyType])) { return NodeStatus.Success; }
                    return NodeStatus.Failure;
            }
            throw new System.Exception("It should never hapeen?");
        }
    }
}
