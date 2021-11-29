using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionGroup : Condition
{
    /// <summary>
    /// �жϵ�Ԫ
    /// </summary>
    [System.Serializable]
    private class JudgeCell
    {
        public Condition Condition;
        /// <summary>
        /// λ��������
        /// </summary>
        [System.Serializable]
        public enum MergeType
        {
            AND,
            OR,
            XOR,
        }
        /// <summary>
        /// �Ƿ�ȡ��
        /// </summary>
        public bool Inverse;
        /// <summary>
        /// ����һ�ڵ�����㷽ʽ
        /// </summary>
        public MergeType Type;
    }

    /// <summary>
    /// ��ǰ������ı��ʽ
    /// </summary>
    [SerializeField]
    private List<JudgeCell> _expression;

    /// <summary>
    /// �Ƿ�ȡ��
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
