using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace NPC
{
    /// <summary>
    /// ½ÇÉ«×´Ì¬¹ý¶É
    /// </summary>
    public abstract class BaseTransition : SerializedScriptableObject
    {
        public abstract bool Reason(BehaviorStateMachine user);
    }
}