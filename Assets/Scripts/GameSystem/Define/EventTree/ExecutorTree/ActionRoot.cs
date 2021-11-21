using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EventTree 
{

    public class ActionRoot : ActionNode
    {
        public override bool IsRoot => true;

        protected override void EventHandler(string eventName, EventCenter.EventArgs eventArgs)
        {
            
        }
    }
}