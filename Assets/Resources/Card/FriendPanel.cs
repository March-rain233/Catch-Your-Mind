using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FriendPanel : MonoBehaviour
{
    public CanvasGroup Default;
    public CanvasGroup Yue;
    public CanvasGroup Dong;
    public Card Card;
    public List<Button> Buttons;

    private void Awake()
    {
        Yue.alpha = Dong.alpha = 0;
        Yue.blocksRaycasts = Dong.blocksRaycasts = false;
    }
}
