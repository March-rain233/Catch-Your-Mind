using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPC
{
    /// <summary>
    /// 选择器
    /// </summary>
    public class SelectorNode : CompositeNode
    {
        /// <summary>
        /// 当前运行节点
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
            //检查打断
            for (int i = 0; i < _current; ++i)
            {
                //如果该节点打断下方则检测高优先度条件节点，若存在返回成功则直接打断返回成功
                if (AbortType == AbortType.Self || AbortType == AbortType.Both)
                {
                    var condition = Childrens[i] as ConditionNode;
                    if (condition && condition.Tick(runner) == NodeStatus.Success)
                    {
                        AbortAllRunningNode(runner, new List<Node>() { Childrens[i] });
                        return NodeStatus.Success;
                    }
                }
                //如果子结合节点打断右方则，若存在返回成功则直接打断返回成功
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