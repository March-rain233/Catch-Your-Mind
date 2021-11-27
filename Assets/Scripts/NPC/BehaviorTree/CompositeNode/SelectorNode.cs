using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPC
{
    /// <summary>
    /// ѡ����
    /// </summary>
    public class SelectorNode : CompositeNode
    {
        /// <summary>
        /// ��ǰ���нڵ�
        /// </summary>
        private int _current;

        protected override void OnEnter(BehaviorTreeRunner runner)
        {
            _current = 0;
            //_current = Childrens.FindIndex(child => child.Status == NodeStatus.Aborting);
            //if (_current == -1) { _current = 0; }
        }

        protected override NodeStatus OnUpdate(BehaviorTreeRunner runner)
        {
            //�����
            for (int i = 0; i < _current; ++i)
            {
                //����ýڵ����·���������ȶ������ڵ㣬�����ڷ��سɹ���ֱ�Ӵ�Ϸ��سɹ�
                if (AbortType == AbortType.Self || AbortType == AbortType.Both)
                {
                    var condition = Childrens[i] as ConditionNode;
                    if (condition && condition.Tick(runner) == NodeStatus.Success)
                    {
                        AbortAllRunningNode(runner, new List<Node>() { Childrens[i] });
                        return NodeStatus.Success;
                    }
                }
                //����ӽ�Ͻڵ����ҷ��������ڷ��سɹ���ֱ�Ӵ�Ϸ��سɹ�
                var composite = Childrens[i] as CompositeNode;
                if (composite && (composite.AbortType == AbortType.LowerPriority
                    || composite.AbortType == AbortType.Both))
                {
                    var s = composite.Tick(runner);
                    if (s == NodeStatus.Running || s == NodeStatus.Success)
                    {
                        AbortAllRunningNode(runner, new List<Node>() { Childrens[i] });
                        return s;
                    }
                }
            }

            var status = Childrens[_current].Tick(runner);
            while (status == NodeStatus.Failure && ++_current < Childrens.Count)
            {
                status = Childrens[_current].Tick(runner);
            }
            return status;
        }
    }
}