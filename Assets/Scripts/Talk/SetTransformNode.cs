using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Dialogue
{

    public class SetTransformNode : ActionNode
    {
        [System.Serializable]
        public struct TransformInfo
        {
            public string Target;
            public string Source;

            public Vector3 Posiotion;
            public Vector3 Rotation;
            public Vector3 Scale;

            public bool IgnorePosition;
            public bool IgnoreRotation;
            public bool IgnoreScale;
        }

        public List<TransformInfo> TransformInfos;

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
            TransformInfos.ForEach(p =>
            {
                if (string.IsNullOrEmpty(p.Source))
                {
                    var t = FindTransform(tree, p.Target);
                    if(!p.IgnorePosition) t.position = p.Posiotion;
                    if (!p.IgnoreRotation) t.eulerAngles = p.Rotation;
                    if (!p.IgnoreScale) t.localScale = p.Scale;
                }
                else
                {
                    var t = FindTransform(tree, p.Target);
                    var s = FindTransform(tree, p.Source);
                    if (!p.IgnorePosition) t.position = s.position;
                    if (!p.IgnoreRotation) t.eulerAngles = s.eulerAngles;
                    if (!p.IgnoreScale) t.localScale = s.localScale;
                }
            });

            return NodeStatus.Success;
        }
    }
}