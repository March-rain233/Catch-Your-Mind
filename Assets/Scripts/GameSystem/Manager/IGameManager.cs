using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 游戏管理器接口
/// </summary>
public abstract class IGameManager : MonoBehaviour 
{
    /// <summary>
    /// 当前场景
    /// </summary>
    public Config.SceneInfo CurrentScene
    {
        get;
        protected set;
    }
    public GameStatus Status
    {
        get;
        protected internal set;
    }
}
