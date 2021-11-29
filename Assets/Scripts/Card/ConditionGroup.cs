using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionGroup : Condition
{
    /// <summary>
    /// 判断单元
    /// </summary>
    [System.Serializable]
    private class JudgeCell
    {
        public Condition Condition;
        /// <summary>
        /// 位运算类型
        /// </summary>
        [System.Serializable]
        public enum MergeType
        {
            AND,
            OR,
            XOR,
        }
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
    public override bool Reason()
    {
        bool value;
        if (_expression.Count <= 0) { return Inverse; }

        value = _expression[0].Inverse ^ _expression[0].Condition.Reason();
        for (int i = 1; i < _expression.Count; ++i)
        {
            switch (_expression[i].Type)
            {
                case JudgeCell.MergeType.AND:
                    value &= (_expression[i].Inverse ^ _expression[i].Condition.Reason());
                    break;
                case JudgeCell.MergeType.OR:
                    value |= (_expression[i].Inverse ^ _expression[i].Condition.Reason());
                    break;
                case JudgeCell.MergeType.XOR:
                    value ^= (_expression[i].Inverse ^ _expression[i].Condition.Reason());
                    break;
            }
        }
        return value;
    }
}
