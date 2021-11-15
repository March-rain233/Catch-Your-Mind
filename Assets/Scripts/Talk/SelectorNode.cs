using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogue
{
    public class SelectorNode : CompositeNode
    {
        protected override Node SelectChild(DialogueTree tree)
        {
            return Childrens.Find(child => child.JudgeCondition(tree));
        }
    }
}
