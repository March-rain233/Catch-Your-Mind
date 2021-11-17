using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPC
{

    public class CheckNode : ConditionNode
    {
        [SerializeField]
        private string _valueName;
        protected override NodeStatus OnUpdate(BehaviorTreeRunner runner)
        {
            if(Invert ^ (runner.Variables.ContainsKey(_valueName) && runner.Variables[_valueName].Boolean))
            {
                return NodeStatus.Success;
            }
            return NodeStatus.Failure;
        }
    }
}
