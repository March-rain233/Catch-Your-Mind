using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPC
{
    /// <summary>
    /// 组合节点
    /// </summary>
    public abstract class CompositeNode : Node
    {

        /// <summary>
        /// 该节点打断方式
        /// </summary>
        public AbortType AbortType => _abortType;
        [SerializeField]
        private AbortType _abortType;

        public List<Node> Childrens = new List<Node>();

        public override UnityEditor.Experimental.GraphView.Port.Capacity Output => UnityEditor.Experimental.GraphView.Port.Capacity.Multi;

        protected override void OnAbort(BehaviorTreeRunner runner)
        {
            Childrens.ForEach(child => { if (child.Status == NodeStatus.Running) child.Abort(runner); });
        }

        public override INode[] GetChildren()
        {
            return Childrens.ToArray();
        }

        protected override void OnEnter(BehaviorTreeRunner runner)
        {

        }

        protected override void OnExit(BehaviorTreeRunner runner)
        {
            OnAbort(runner);
        }

        protected override void OnResume(BehaviorTreeRunner runner)
        {

        }

        protected void AbortAllRunningNode(BehaviorTreeRunner runner, List<Node> except)
        {
            Childrens.ForEach(child =>
            {
                if (child.Status == NodeStatus.Running && !except.Contains(child))
                {
                    child.Abort(runner);
                }
            });
        }

        public override Node Clone(bool self = false)
        {
            var node = base.Clone() as CompositeNode;
            node.Childrens = new List<Node>(Childrens);
            if (self) { return node; }
            for (int i = Childrens.Count - 1; i >= 0; --i)
            {
                if (!Childrens[i])
                {
                    Childrens.RemoveAt(i);
                    node.Childrens.RemoveAt(i);
                    continue;
                }
                node.Childrens[i] = Childrens[i].Clone();
            }
            return node;
        }
    }
    /// <summary>
    /// 打断方式
    /// </summary>
    [System.Serializable]
    public enum AbortType
    {
        None,
        LowerPriority,
        Self,
        Both
    }
}