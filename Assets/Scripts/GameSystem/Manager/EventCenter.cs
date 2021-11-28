using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Map;

/// <summary>
/// 事件中心，用于提交驱动游戏的事件
/// </summary>
[System.Serializable]
public class EventCenter : SerializedMonoBehaviour
{
    /// <summary>
    /// 用于事件参数
    /// </summary>
    [System.Serializable]
    public struct EventArgs
    {
        public float Float;
        public int Int;
        public bool Boolean;
        public string String;
        public object Object;
    }

    /// <summary>
    /// 消息通道
    /// </summary>
    [TabGroup("数据库", "事件"), DictionaryDrawerSettings(KeyLabel = "事件名", ValueLabel = "事件参数", DisplayMode = DictionaryDisplayOptions.OneLine), SerializeField,]
    private Dictionary<string, EventArgs> _events = new Dictionary<string, EventArgs>();

    /// <summary>
    /// 消息更新
    /// </summary>
    public event System.Action<string, EventArgs> EventChanged;

    /// <summary>
    /// 监听者列表
    /// </summary>
    private Dictionary<string, List<System.Action<EventArgs>>> _listeners = new Dictionary<string, List<System.Action<EventArgs>>>();

    public void Init(Dictionary<string, EventArgs> save)
    {
        _events = save;
    }

    /// <summary>
    /// 获取事件的参数
    /// </summary>
    /// <param name="eventName"></param>
    public bool TryGetEventArgs(string eventName, out EventArgs eventArgs)
    {
        if(_events.ContainsKey(eventName))
        {
            eventArgs = _events[eventName];
            return true;
        }
        eventArgs = default;
        return false;
    }

    /// <summary>
    /// 发送事件
    /// </summary>
    /// <param name="eventName"></param>
    /// <param name="eventArgs"></param>
    public void SendEvent(string eventName, EventArgs eventArgs)
    {
        if (!_events.ContainsKey(eventName))
        {
            _events.Add(eventName, eventArgs);
        }
        else
        {
            _events[eventName] = eventArgs;
        }

        EventChanged?.Invoke(eventName, eventArgs);
        if (!_listeners.ContainsKey(eventName)) { return; }

        foreach(var h in _listeners[eventName])
        {
            h.Invoke(eventArgs);
        }
    }

    /// <summary>
    /// 添加监听者
    /// </summary>
    /// <param name="eventName"></param>
    /// <param name="callback"></param>
    public void AddListener(string eventName, System.Action<EventArgs> callback)
    {
        if (!_listeners.ContainsKey(eventName))
        {
            _listeners.Add(eventName, new List<System.Action<EventArgs>>());
        }
        _listeners[eventName].Add(callback);
    }

    /// <summary>
    /// 移除监听者
    /// </summary>
    /// <param name="eventName"></param>
    /// <param name="callback"></param>
    public void RemoveListener(string eventName, System.Action<EventArgs> callback)
    {
        _listeners[eventName].Remove(callback);
    }
}
