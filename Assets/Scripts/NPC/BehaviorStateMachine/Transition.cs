using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPC
{
    /// <summary>
    /// 过渡
    /// </summary>
    public class Transition : ScriptableObject
    {
        [System.Serializable]
        public struct ConditionInfo 
        {
            public bool Inverse;
            public BaseCondition Condition;
        }

        /// <summary>
        /// 起始状态
        /// </summary>
        public BaseState StartState;

        /// <summary>
        /// 目标状态
        /// </summary>
        public BaseState EndState;

        /// <summary>
        /// 条件
        /// </summary>
        public List<ConditionInfo> Conditions;

        /// <summary>
        /// 判断
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool Reason(BehaviorStateMachine user)
        {
            bool value = true;
            foreach(var condition in Conditions)
            {
                value &= condition.Inverse ^ condition.Condition.Reason(user);
            }
            return value;
        }
    }
}
