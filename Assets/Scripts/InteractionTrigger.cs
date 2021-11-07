using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 场景互动触发器
/// </summary>
public class InteractionTrigger : MonoBehaviour
{
    /// <summary>
    /// 事件参数
    /// </summary>
    public EventCenter.EventArgs EventArgs
    {
        get { return _eventArgs; }
    }
    [SerializeField]
    private EventCenter.EventArgs _eventArgs;

    /// <summary>
    /// 事件名称
    /// </summary>
    public string EventName
    {
        get { return _eventName; }
    }
    [SerializeField]
    private string _eventName;

    /// <summary>
    /// 交互按键
    /// </summary>
    public KeyType InteractKey
    {
        get { return _interactKey; }
    }
    [SerializeField]
    private KeyType _interactKey;

    /// <summary>
    /// 事件触发
    /// </summary>
    public UnityAction<string, EventCenter.EventArgs> EventTriggered;

    /// <summary>
    /// 玩家进入事件
    /// </summary>
    public UnityAction<InteractionTrigger> PlayerEnter;

    /// <summary>
    /// 玩家离开事件
    /// </summary>
    public UnityAction<InteractionTrigger> PlayerExit;

    private void Awake()
    {
        _eventArgs.Object = this;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) 
        {
            PlayerEnter?.Invoke(this);
            GameManager.Instance.EventCenter.SendEvent("InteractionEnter", EventArgs);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerExit?.Invoke(this);
            GameManager.Instance.EventCenter.SendEvent("InteractionExit", EventArgs);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") 
            && Input.GetKeyDown(GameManager.Instance.ControlManager.KeyDic[InteractKey]))
        {
            EventTriggered?.Invoke(EventName, EventArgs);
            GameManager.Instance.EventCenter.SendEvent(EventName, EventArgs);
        }
    }
}
