using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPC
{
    public class TriggerSetterNode : ActionNode
    {
        /// <summary>
        /// 要设定的trigger的名字
        /// </summary>
        [SerializeField]
        private string _triggerName;

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
            (runner.Variables["Animator"].Object as Animator).SetTrigger(_triggerName);
            return NodeStatus.Success;
        }
    }
}
