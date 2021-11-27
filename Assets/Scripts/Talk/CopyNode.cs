using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogue {

    public class CopyNode : ActionNode
    {
        [System.Serializable]
        public struct CopyInfo
        {
            public string Target;
            public string Source;
        }

        public List<CopyInfo> CopyInfos;

        protected override NodeStatus OnUpdate(DialogueTree tree)
        {
            CopyInfos.ForEach(c => tree.Variables[c.Target] = tree.Variables[c.Source]);
            return NodeStatus.Success;
        }
    }
}
