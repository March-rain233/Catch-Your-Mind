using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPC
{
    [CreateAssetMenu(fileName = "条件组", menuName = "角色/条件/条件组")]
    public class GroupCondition : BaseCondition
    {
        /// <summary>
        /// 判断单元
        /// </summary>
        [System.Serializable]
        private class JudgeCell
        {
            /// <summary>
            /// 位运算类型
            /// </summary>
            [System.Serializable]
            internal enum MergeType
            {
                AND,
                OR,
                XOR,
            }
            /// <summary>
            /// 当前节点的过渡单元
            /// </summary>
            public BaseCondition Transition;
            /// <summary>
            /// 是否取反
            /// </summary>
            public bool Inverse;
            /// <summary>
            /// 与上一节点的运算方式
            /// </summary>
            public MergeType Type;
        }

        /// <summary>
        /// 当前过渡组的表达式
        /// </summary>
        [SerializeField]
        private List<JudgeCell> _expression;

        /// <summary>
        /// 是否取反
        /// </summary>
        public bool Inverse;
        public override bool Reason(BehaviorStateMachine user)
        {
            bool value;
            if(_expression.Count <= 0) { return Inverse; }

            value = _expression[0].Inverse ^ _expression[0].Transition.Reason(user);
            for(int i = 1; i < _expression.Count; ++i)
            {
                switch (_expression[i].Type)
                {
                    case JudgeCell.MergeType.AND:
                        value &= (_expression[i].Inverse ^ _expression[i].Transition.Reason(user));
                        break;
                    case JudgeCell.MergeType.OR:
                        value |= (_expression[i].Inverse ^ _expression[i].Transition.Reason(user));
                        break;
                    case JudgeCell.MergeType.XOR:
                        value ^= (_expression[i].Inverse ^ _expression[i].Transition.Reason(user));
                        break;
                }
            }
            return value;
        }
    }
}
