using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPC
{
    public abstract class ConditionNode : Leaf
    {
        public bool Invert = false;

        protected override void OnEnter(BehaviorTreeRunner runner)
        {
        }

        protected override void OnExit(BehaviorTreeRunner runner)
        {
        }

        protected override void OnResume(BehaviorTreeRunner runner)
        {
        }

        protected override void OnAbort(BehaviorTreeRunner runner)
        {
        }
    }
}
