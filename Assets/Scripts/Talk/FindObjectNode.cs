using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogue
{

    public class FindObjectNode : ActionNode
    {
        [System.Serializable]
        public struct ObjectInfo
        {
            public string Path;
            public string Name;
        }

        public List<ObjectInfo> ObjectInfos;

        protected override NodeStatus OnUpdate(DialogueTree tree)
        {
            ObjectInfos.ForEach(o =>
            {
                var obj = GameObject.Find(o.Path);
                tree.Variables[o.Name] = new EventCenter.EventArgs() { Object = obj };
            });
            return NodeStatus.Success;
        }
    }
}