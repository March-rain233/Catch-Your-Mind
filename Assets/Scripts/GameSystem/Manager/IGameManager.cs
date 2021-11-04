using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��Ϸ�������ӿ�
/// </summary>
public abstract class IGameManager : MonoBehaviour 
{
    /// <summary>
    /// ��ǰ����
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
