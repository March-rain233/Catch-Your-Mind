using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace NPC
{
    public class MindIdleNode : ActionNode
    {
        private Animator _animator;
        private Rigidbody2D _rg;

        protected override void OnEnter(BehaviorTreeRunner runner)
        {
            _animator = runner.Variables["Animator"] as Animator;
            _rg = runner.Variables["Rigidbody"] as Rigidbody2D;
            _animator.SetBool("IsIdle", true);
            _rg.velocity = Vector2.zero;
        }

        protected override void OnExit(BehaviorTreeRunner runner)
        {
            _animator.SetBool("IsIdle", false);
        }

        protected override void OnAbort(BehaviorTreeRunner runner)
        {
            OnExit(runner);
        }

        protected override void OnResume(BehaviorTreeRunner runner)
        {
            OnEnter(runner);
        }

        protected override NodeStatus OnUpdate(BehaviorTreeRunner runner)
        {
            _rg.velocity = Vector2.zero;
            return NodeStatus.Running;
        }
    }
}
