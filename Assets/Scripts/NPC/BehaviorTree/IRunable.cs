using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRunable
{
    NodeStatus Tick(NPC.BehaviorTreeRunner runner);
    IRunable Clone();
}
