using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Dialogue
{

    public class DestoryNode : ActionNode
    {
        [System.Serializable]
        public struct ObjectInfo
        {
            public string Name;
            public float Remain;
        }

        public List<ObjectInfo> Objects;

        protected override void OnEnter(DialogueTree tree)
        {
            base.OnEnter(tree);
        }

        protected override NodeStatus OnUpdate(DialogueTree tree)
        {
            Objects.ForEach(obj =>
            {
                GameObject t = null;
                if (tree.Variables.ContainsKey(obj.Name))
                {
                    var temp = tree.Variables[obj.Name].Object;
                    if (temp is GameObject)
                    {
                        t = temp as GameObject;
                    }
                    if (temp is Component)
                    {
                        t = (temp as Component).gameObject;
                    }
                }
                else
                {
                    t = GameObject.Find(obj.Name);
                }
                Destroy(t, obj.Remain);
            });
            return NodeStatus.Success;
        }
    }
}
