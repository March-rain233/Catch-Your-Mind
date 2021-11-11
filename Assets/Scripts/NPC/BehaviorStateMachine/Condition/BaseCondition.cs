using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace NPC
{
    /// <summary>
    /// 状态机条件基类
    /// </summary>
    public abstract class BaseCondition : SerializedScriptableObject
    {
        public abstract bool Reason(BehaviorStateMachine user);
    }
}