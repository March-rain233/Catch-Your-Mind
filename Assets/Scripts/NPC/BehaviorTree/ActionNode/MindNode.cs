using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPC
{
    public abstract class MindNode : ActionNode
    {
        protected Animator _animator;
        protected Rigidbody2D _rg;
        protected NPC_Model _model;
        [SerializeField]
        protected string _triggerName;

        protected override void OnEnter(BehaviorTreeRunner runner)
        {
            _animator = runner.Variables["Animator"].Object as Animator;
            _rg = runner.Variables["Rigidbody"].Object as Rigidbody2D;
            _model = runner.Variables["Model"].Object as NPC_Model;
            //Debug.Log(_triggerName);
            _animator.SetTrigger(_triggerName);
        }

        protected override void OnExit(BehaviorTreeRunner runner)
        {
            _animator.ResetTrigger(_triggerName);
        }

        protected override void OnAbort(BehaviorTreeRunner runner)
        {
            OnExit(runner);
        }

        protected override void OnResume(BehaviorTreeRunner runner)
        {
            OnEnter(runner);
        }
    }
}