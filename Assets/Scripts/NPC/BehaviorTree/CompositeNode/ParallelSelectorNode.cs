using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPC
{
    /// <summary>
    /// 并行选择节点
    /// </summary>
    public class ParallelSelectorNode : CompositeNode
    {

        protected override NodeStatus OnUpdate(BehaviorTreeRunner runner)
        {
            bool hasRunning = false;
            bool hasSuccess = false;
            foreach(var child in Childrens)
            {
                switch (child.Tick(runner))
                {
                    case NodeStatus.Success:
                        hasSuccess = true;
                        break;
                    case NodeStatus.Running:
                        hasRunning = true;
                        break;
                }
            }
            if (hasSuccess) return NodeStatus.Success;
            if (hasRunning) return NodeStatus.Running;
            return NodeStatus.Failure;
        }
    }
}