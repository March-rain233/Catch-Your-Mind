using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace NPC
{

    public class LogNode : ActionNode
    {
        [SerializeField]
        private string _log;

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
            Debug.Log(_log);
            return NodeStatus.Success;
        }
    }
}
