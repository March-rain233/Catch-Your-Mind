using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogue
{
    public class RootNode : ActionNode
    {
        public override bool IsRoot => true;

        protected override NPC.Node.NodeStatus OnUpdate(DialogueTree tree)
        {
            return NPC.Node.NodeStatus.Success;
        }
    }
}
