using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace NPC
{
    public abstract class Leaf : Node
    {
        public override bool IsLeaf => true;

#if UNITY_EDITOR
        public override UnityEditor.Experimental.GraphView.Port.Capacity Output => UnityEditor.Experimental.GraphView.Port.Capacity.Single;
#endif

        public override INode[] GetChildren()
        {
            //·µ»Ø¿ÕÊý×é
            return new INode[] { };
        }
    }
}
