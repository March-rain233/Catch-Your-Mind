using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogue
{

    public abstract class CompositeNode : Node
    {
        public override UnityEditor.Experimental.GraphView.Port.Capacity Output => UnityEditor.Experimental.GraphView.Port.Capacity.Multi;

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

        protected override NPC.Node.NodeStatus OnUpdate(DialogueTree tree)
        {
            return NPC.Node.NodeStatus.Success;
        }
    }
}
