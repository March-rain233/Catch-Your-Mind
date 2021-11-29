using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterTrigger : MonoBehaviour
{
    public string Name;
    public EventCenter.EventArgs EventArgs;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameManager.Instance.EventCenter.SendEvent(Name, EventArgs);
        }
    }
}
