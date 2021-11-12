using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Leaf : Node
{
    public override bool IsLeaf => true;

    public override INode[] GetChildren()
    {
        //·µ»Ø¿ÕÊı×é
        return new INode[] { };
    }
}
