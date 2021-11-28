using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventCondition : Condition
{
    public struct EventInfo
    {
        public string EventName;
        public bool Interval;
    }

    public List<EventInfo> eventInfos;

    public override bool Reason()
    {
        foreach(var e in eventInfos)
        {
            if (!((GameManager.Instance.EventCenter.TryGetEventArgs(e.EventName, out EventCenter.EventArgs eventArgs) && eventArgs.Boolean) ^ e.Interval))
            {
                return false;
            }
        }
        return true;
    }
}
