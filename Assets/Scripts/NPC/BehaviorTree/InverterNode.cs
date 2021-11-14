using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPC
{
    /// <summary>
    /// ȡ����
    /// </summary>
    public class InverterNode : DecoratorNode
    {
        protected override NodeStatus OnUpdate(BehaviorTreeRunner runner)
        {
            switch (Child.Tick(runner))
            {
                case NodeStatus.Success:
                    return NodeStatus.Failure;
                case NodeStatus.Failure:
                    return NodeStatus.Success;
                case NodeStatus.Running:
                    break;
            }

            throw new System.Exception("������δ֪�Ĵ���");
        }
    }
}
