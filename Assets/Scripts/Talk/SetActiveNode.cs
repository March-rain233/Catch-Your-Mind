using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogue
{

    public class SetActiveNode : ActionNode
    {
        [System.Serializable]
        public struct ObjectInfo
        {
            public string Name;
            public bool TargetValue;
        }

        public List<ObjectInfo> ObjectList;

        protected override NodeStatus OnUpdate(DialogueTree tree)
        {
            ObjectList.ForEach(obj =>
            {
                GameObject o;
                if (tree.Variables.ContainsKey(obj.Name))
                {
                    o = tree.Variables[obj.Name].Object as GameObject;
                }
                else
                {
                    o = GameObject.Find(obj.Name);
                    tree.Variables.Add(obj.Name, new EventCenter.EventArgs() { Object = o });
                }
                o.SetActive(obj.TargetValue);
            });
            return NodeStatus.Success;
        }
    }
}
