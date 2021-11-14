using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 修饰节点
/// </summary>
namespace NPC
{
    public abstract class DecoratorNode : Node
    {
        /// <summary>
        /// 子节点
        /// </summary>
        public Node Child;

        public override UnityEditor.Experimental.GraphView.Port.Capacity Output => UnityEditor.Experimental.GraphView.Port.Capacity.Single;

        protected override void OnEnter(BehaviorTreeRunner runner)
        {

        }

        protected override void OnExit(BehaviorTreeRunner runner)
        {
        }

        protected override void OnResume(BehaviorTreeRunner runner)
        {
        }

        protected override void OnAbort(BehaviorTreeRunner runner)
        {
            if (Child.Status == NodeStatus.Running) { Child.Abort(runner); }
        }

        public override INode[] GetChildren()
        {
            if (Child) return new INode[] { Child };
            else return new INode[] { };
        }

        public override Node Clone()
        {
            var node = base.Clone() as DecoratorNode;
            node.Child = Child.Clone();
            return node;
        }
    }
}
