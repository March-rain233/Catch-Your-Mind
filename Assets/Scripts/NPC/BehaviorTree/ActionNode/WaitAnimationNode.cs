using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPC {

    public class WaitAnimationNode : ActionNode
    {
        public string Name;
        public float Time;

        private Animator _animator;

        protected override void OnAbort(BehaviorTreeRunner runner)
        {
            OnExit(runner);
        }

        protected override void OnEnter(BehaviorTreeRunner runner)
        {
            _animator = runner.Variables["Animator"].Object as Animator;
            //_animator.Play(Name);
        }

        protected override void OnExit(BehaviorTreeRunner runner)
        {

        }

        protected override void OnResume(BehaviorTreeRunner runner)
        {
            OnEnter(runner);
        }

        protected override NodeStatus OnUpdate(BehaviorTreeRunner runner)
        {
            var state = _animator.GetCurrentAnimatorStateInfo(0);
            if (state.IsName(Name))
            {
                if (state.normalizedTime >= Time)
                {
                    return NodeStatus.Success;
                }
                return NodeStatus.Running;
            }
            return NodeStatus.Failure;
        }
    }
}
