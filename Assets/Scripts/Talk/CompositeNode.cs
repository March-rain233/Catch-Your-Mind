using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor.Experimental.GraphView;
#endif

namespace Dialogue
{

    public abstract class CompositeNode : Node
    {
#if UNITY_EDITOR
        public override Port.Capacity Output => Port.Capacity.Multi;
#endif
        public List<Node> Childrens;

        public override Node Clone()
        {
            var node = base.Clone() as CompositeNode;
            for (int i = Childrens.Count - 1; i >= 0; --i)
            {
                if (!Childrens[i])
                {
                    Childrens.RemoveAt(i);
                    node.Childrens.RemoveAt(i);
                }
                node.Childrens[i] = Childrens[i].Clone();
            }
            return node;
        }

        public override INode[] GetChildren()
        {
            return Childrens.ToArray();
        }

        protected override NodeStatus OnUpdate(DialogueTree tree)
        {
            return NodeStatus.Success;
        }
    }
}
