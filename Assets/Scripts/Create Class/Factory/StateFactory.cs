using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 状态工厂
/// </summary>
public class StateFactory
{
    /// <summary>
    /// 通过传入的创建脚本来创建状态
    /// </summary>
    /// <param name="method"></param>
    /// <returns></returns>
    public NPC.BaseState CreatState(string method)
    {
        return default;
    }
}
