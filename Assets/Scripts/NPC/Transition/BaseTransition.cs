using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace NPC
{
    /// <summary>
    /// ��ɫ״̬����
    /// </summary>
    public abstract class BaseTransition : SerializedScriptableObject
    {
        public abstract bool Reason(BehaviorStateMachine user);
    }
}