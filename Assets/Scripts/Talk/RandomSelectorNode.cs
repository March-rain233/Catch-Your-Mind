using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogue
{
    /// <summary>
    /// Ëæ»úÑ¡ÔñÆ÷
    /// </summary>
    public class RandomSelectorNode : CompositeNode
    {
        protected override Node SelectChild(DialogueTree tree)
        {
            var toRandom = Childrens.FindAll(child => child.JudgeCondition(tree));
            if (toRandom.Count == 0) { return null; }
            int index = Random.Range(0, toRandom.Count);
            return toRandom[index];
        }
    }
}
