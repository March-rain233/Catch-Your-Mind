using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPC
{

    public class ResetTriggerNode : ActionNode
    {
        /// <summary>
        /// 要设定的trigger的名字
        /// </summary>
        [SerializeField]
        private string[] _triggerNames;

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
            var anim = runner.Variables["Animator"].Object as Animator;
            System.Array.ForEach(_triggerNames, trigger => anim.ResetTrigger(trigger));
            return NodeStatus.Success;
        }
    }
}
