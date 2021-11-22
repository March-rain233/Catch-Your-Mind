using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

/// <summary>
/// 控制管理器，负责管控键位
/// </summary>
public class ControlManager : SerializedMonoBehaviour
{
    /// <summary>
    /// 按键映射字典
    /// </summary>
    public Dictionary<KeyType, KeyCode> KeyDic
    {
        get
        {
            if (_keyDic == null) 
            {
                ResetKey();
            }
            return _keyDic;
        }
    }
    [DictionaryDrawerSettings(KeyLabel = "按键名称", ValueLabel = "按键映射", DisplayMode = DictionaryDisplayOptions.OneLine), SerializeField]
    private Dictionary<KeyType, KeyCode> _keyDic;

    /// <summary>
    /// 初始化
    /// </summary>
    public void Init(Dictionary<KeyType, KeyCode> keyDic)
    {
        if (keyDic == null)
        {
            ResetKey();
        }
        else
        {
            _keyDic = keyDic;
        }
    }

    /// <summary>
    /// 重置按键映射
    /// </summary>
    public void ResetKey()
    {
        _keyDic = new Dictionary<KeyType, KeyCode>
        {
            {KeyType.Interact, KeyCode.E },
            {KeyType.Skip, KeyCode.LeftControl },
            {KeyType.Jump, KeyCode.W },
            {KeyType.Left, KeyCode.A },
            {KeyType.Right, KeyCode.D },
            {KeyType.Run, KeyCode.LeftShift },
            {KeyType.Esc, KeyCode.Escape }
        };
    }

}
