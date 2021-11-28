using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 心动培养基 : MonoBehaviour
{
    public GameObject Default;
    public GameObject Yue;
    public MyButton MyButton;
    public Card Card;

    private void Awake()
    {
        if(GameManager.Instance.EventCenter.TryGetEventArgs("置顶的聊天记录", out EventCenter.EventArgs eventArgs))
        {
            Yue.SetActive(true);
            Default.SetActive(false);
        }
        else
        {
            Yue.SetActive(false);
            Default.SetActive(true);
        }
        MyButton.OnClick += EnableCard;
    }

    private void EnableCard()
    {
        Card.TruthUnable = false;
        Card.InsertUnable = false;
    }
}
