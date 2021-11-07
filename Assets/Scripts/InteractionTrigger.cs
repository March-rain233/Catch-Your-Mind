using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// ��������������
/// </summary>
public class InteractionTrigger : MonoBehaviour
{
    /// <summary>
    /// �¼�����
    /// </summary>
    public EventCenter.EventArgs EventArgs
    {
        get { return _eventArgs; }
    }
    [SerializeField]
    private EventCenter.EventArgs _eventArgs;

    /// <summary>
    /// �¼�����
    /// </summary>
    public string EventName
    {
        get { return _eventName; }
    }
    [SerializeField]
    private string _eventName;

    /// <summary>
    /// ��������
    /// </summary>
    public KeyType InteractKey
    {
        get { return _interactKey; }
    }
    [SerializeField]
    private KeyType _interactKey;

    /// <summary>
    /// �¼�����
    /// </summary>
    public UnityAction<string, EventCenter.EventArgs> EventTriggered;

    /// <summary>
    /// ��ҽ����¼�
    /// </summary>
    public UnityAction<InteractionTrigger> PlayerEnter;

    /// <summary>
    /// ����뿪�¼�
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
