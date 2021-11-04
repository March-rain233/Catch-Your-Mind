using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace NPC
{
    /// <summary>
    /// 增益效果
    /// </summary>
    [Flags, Serializable]
    public enum Buff : long
    {
        Recover_1 = 1,
        Recover_2 = Recover_1 << 1,
        Recover_3 = Recover_2 << 1,
        Empyrosis_1 = Recover_3 << 1,
        Empyrosis_2 = Empyrosis_1 << 1,
        Empyrosis_3 = Empyrosis_2 << 1,
    }
}
