using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPC
{

    public class SetTimeNode : ActionNode
    {

        [SerializeField]
        private string _valueName;

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
            if(runner.Variables.TryGetValue(_valueName, out EventCenter.EventArgs temp))
            {
                temp.Float = Time.time;
                runner.Variables[_valueName] = temp;
            }
            else
            {
                runner.Variables.Add(_valueName, new EventCenter.EventArgs() { Float = Time.time });
            }
            return NodeStatus.Success;
        }
    }
}
