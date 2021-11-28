using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogue
{

    public class AnimatorSpeedNode : ActionNode
    {
        [System.Serializable]
        public struct AnimatorInfo
        {
            public string Name;
            public float Speed;
        }

        public List<AnimatorInfo> AnimatorInfos;

        private Transform FindTransform(DialogueTree tree, string name)
        {
            GameObject o;
            if (tree.Variables.ContainsKey(name))
            {
                o = tree.Variables[name].Object as GameObject;
            }
            else
            {
                o = GameObject.Find(name);
                tree.Variables.Add(name, new EventCenter.EventArgs() { Object = o });
            }
            return o.transform;
        }

        protected override NodeStatus OnUpdate(DialogueTree tree)
        {
            AnimatorInfos.ForEach(a =>
            {
                var animator = FindTransform(tree, a.Name).GetComponent<Animator>();
                animator.speed = a.Speed;
            });
            return NodeStatus.Success;
        }
    }
}