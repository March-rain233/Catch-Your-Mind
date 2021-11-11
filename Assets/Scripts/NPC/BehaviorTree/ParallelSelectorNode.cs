using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 并行选择节点
/// </summary>
public class ParallelSelectorNode : CompositeNode
{

    protected override NodeStatus OnUpdate(BehaviorTreeRunner runner)
    {
        bool isRunning = false;
        bool isAllFalse = true;

        if(Status == NodeStatus.Running)
        {
            Childrens.ForEach(child =>
            {
                if (child.Status == NodeStatus.Running || child.Status == NodeStatus.Aborting)
                {
                    var status = child.Tick(runner);
                    switch (status)
                    {
                        case NodeStatus.Success:
                            isAllFalse = false;
                            break;
                        case NodeStatus.Failure:
                            break;
                        case NodeStatus.Running:
                            isRunning = true;
                            break;
                        case NodeStatus.Aborting:
                            Debug.Log("What happened???");
                            break;
                    }
                }
            });
        }
        else
        {
            Childrens.ForEach(child =>
            {
                var status = child.Tick(runner);
                switch (status)
                {
                    case NodeStatus.Success:
                        isAllFalse = false;
                        break;
                    case NodeStatus.Failure:
                        break;
                    case NodeStatus.Running:
                        isRunning = true;
                        break;
                    case NodeStatus.Aborting:
                        Debug.Log("What happened???");
                        break;
                }
            });
        }

        if (isRunning) { return NodeStatus.Running; }
        else if (isAllFalse) { return NodeStatus.Failure; }
        else { return NodeStatus.Success; }
    }
}
