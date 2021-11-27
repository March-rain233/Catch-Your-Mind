using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace NPC {

    public class TalkNode : ActionNode
    {
        public Dialogue.DialogueTree DialogueTree;

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
            TalkSystem.Instance.DialogueTree = DialogueTree;
            return NodeStatus.Success;
        }
    }
}
