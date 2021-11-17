using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPC
{
    /// <summary>
    /// 判断键盘双击
    /// </summary>
    public class DoubleKeyNode : ConditionNode
    {
        /// <summary>
        /// 检测的按键类型
        /// </summary>
        [SerializeField]
        private KeyType _keyType;

        /// <summary>
        /// 判定间隔
        /// </summary>
        [SerializeField]
        private float _judgeInterval;

        /// <summary>
        /// 上一次按压时间
        /// </summary>
        private float _lastPressTime = 0;

        protected override NodeStatus OnUpdate(BehaviorTreeRunner runner)
        {
            if (UnityEngine.Input.GetKeyDown(GameManager.Instance.ControlManager.KeyDic[_keyType]))
            {
                if(Invert ^ Time.time - _lastPressTime <= _judgeInterval)
                {
                    _lastPressTime = Time.time;
                    return NodeStatus.Success;
                }
                _lastPressTime = Time.time;
            }
            return NodeStatus.Failure;
        }
    }
}
