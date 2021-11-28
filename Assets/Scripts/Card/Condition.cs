using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public abstract class Condition : SerializedScriptableObject
{
    public abstract bool Reason();
}
