using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InteractionArea : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public Texture2D PressIcon;

    public string EventName;

    public EventCenter.EventArgs EventArg;

    public void OnPointerClick(PointerEventData eventData)
    {
        GameManager.Instance.EventCenter.SendEvent(EventName, EventArg);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Cursor.SetCursor(PressIcon, Vector2.zero, CursorMode.Auto);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }
}
