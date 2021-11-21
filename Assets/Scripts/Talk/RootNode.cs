using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogue
{
    public class RootNode : ActionNode
    {
        public override bool IsRoot => true;

        protected override NodeStatus OnUpdate(DialogueTree tree)
        {
            return NodeStatus.Success;
        }
    }
}
