using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogue
{

    public class AnimatorEnableNode : ActionNode
    {
        [System.Serializable]
        public struct AnimatorInfo
        {
            public string Name;
            public bool Enable;
        }

        public List<AnimatorInfo> Animators;

        protected override NodeStatus OnUpdate(DialogueTree tree)
        {
            Animators.ForEach(a =>
            {
                if (tree.Variables.ContainsKey(a.Name))
                {
                    var anim = (tree.Variables[a.Name].Object as GameObject)?.GetComponent<Animator>();
                    if (!anim)
                    {
                        anim = (tree.Variables[a.Name].Object as Component)?.GetComponent<Animator>();
                    }
                    anim.enabled = a.Enable;
                }
                else
                {
                    var anim = GameObject.Find(a.Name);
                    anim.GetComponent<Animator>().enabled = a.Enable;
                    tree.Variables.Add(a.Name, new EventCenter.EventArgs() { Object = anim });
                }
            });
            return NodeStatus.Success;
        }
    }
}
