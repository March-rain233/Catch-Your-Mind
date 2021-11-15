using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.Serialization;

namespace NPC
{
    public class EventSenderNode : ActionNode
    {
        public string EventName;

        [OdinSerialize]
        public EventCenter.EventArgs EventArg;

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
            GameManager.Instance.EventCenter.SendEvent(EventName, EventArg);
            return NodeStatus.Success;
        }
    }
}
