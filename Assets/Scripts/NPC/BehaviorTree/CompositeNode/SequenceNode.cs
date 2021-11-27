using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPC
{
    /// <summary>
    /// ˳��ڵ�
    /// </summary>
    public class SequenceNode : CompositeNode
    {
        /// <summary>
        /// ��ǰ���нڵ�
        /// </summary>
        [SerializeField]
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
                //����ýڵ����·���������ȶ������ڵ㣬�����ڷ���ʧ����ֱ�Ӵ�Ϸ���ʧ��
                if (AbortType == AbortType.Self || AbortType == AbortType.Both)
                {
                    var condition = Childrens[i] as ConditionNode;
                    if (condition && condition.Tick(runner) == NodeStatus.Failure)
                    {
                        AbortAllRunningNode(runner, new List<Node>() { Childrens[i] });
                        return NodeStatus.Failure;
                    }
                }
                //����ӽ�Ͻڵ����ҷ��������ڷ���ʧ����ֱ�Ӵ�Ϸ���ʧ��
                var composite = Childrens[i] as CompositeNode;
                if (composite && (composite.AbortType == AbortType.LowerPriority
                    || composite.AbortType == AbortType.Both))
                {
                    var s = composite.Tick(runner);
                    if (s == NodeStatus.Running || s == NodeStatus.Failure)
                    {
                        AbortAllRunningNode(runner, new List<Node>() { Childrens[i] });
                        return s;
                    }
                }
            }

            var status = Childrens[_current].Tick(runner);
            while (status == NodeStatus.Success && ++_current < Childrens.Count)
            {
                status = Childrens[_current].Tick(runner);
            }
            return status;
        }
    }
}
