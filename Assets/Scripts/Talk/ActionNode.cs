using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogue
{

    public abstract class ActionNode : Node
    {
        [SerializeField]
        public Node Child;

        protected override Node SelectChild(DialogueTree tree)
        {
            return Child;
        }

        public override INode[] GetChildren()
        {
            if (Child == null) { return new INode[] { }; }
            return new INode[] { Child };
        }

        public override Node Clone()
        {
            var node = base.Clone() as ActionNode;
            if (Child)
            {
                node.Child = Child.Clone();
            }
            return node;
        }
    }
}
