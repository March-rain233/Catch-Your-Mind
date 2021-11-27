using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogue
{

    public class SendEventNode : ActionNode
    {
        [System.Serializable]
        public struct EventInfo
        {
            public string EventName;
            public EventCenter.EventArgs EventArg;
        }
        public List<EventInfo> EventArgs;

        protected override NodeStatus OnUpdate(DialogueTree tree)
        {
            EventArgs.ForEach(e => GameManager.Instance.EventCenter.SendEvent(e.EventName, e.EventArg));
            return NodeStatus.Success;
        }
    }
}