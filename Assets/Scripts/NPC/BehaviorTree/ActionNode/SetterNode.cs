using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.Serialization;

namespace NPC
{
    /// <summary>
    /// 设值节点
    /// </summary>
    public class SetterNode : ActionNode
    {
        [OdinSerialize]
        public Dictionary<string, EventCenter.EventArgs> Variables;

        protected override void OnAbort(BehaviorTreeRunner runner)
        {
        }

        protected override void OnEnter(BehaviorTreeRunner runner)
        {
        }

        protected override void OnExit(BehaviorTreeRunner runner)
        {
        }

        protected override void OnResume(BehaviorTreeRunner runner)
        {
        }

        protected override NodeStatus OnUpdate(BehaviorTreeRunner runner)
        {
            foreach(var p in Variables)
            {
                runner.Variables[p.Key] = p.Value;
            }
            return NodeStatus.Success;
        }
    }
}
