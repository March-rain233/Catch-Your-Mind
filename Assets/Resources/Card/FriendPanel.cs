using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class FriendPanel : MonoBehaviour
{
    public CanvasGroup Default;
    public CanvasGroup Yue;
    public CanvasGroup Dong;
    public Card Card;
    public List<Button> Buttons;
    public MyButton MyButton;

    private void Awake()
    {
        Yue.alpha = Dong.alpha = 0;
        Default.alpha = 1;
        Yue.blocksRaycasts = Dong.blocksRaycasts = false;
        Default.blocksRaycasts = true;
        int i = 0;
        Card.TrueState = false;
        Card.TruthUnable = true;
        Card.InsertUnable = true;
        if(GameManager.Instance.EventCenter.TryGetEventArgs("��֮��ȥ�Ķ��", out EventCenter.EventArgs eventArgs) && eventArgs.Boolean)
        {
            Buttons[i].onClick.AddListener(() =>
            {
                Default.DOFade(0, 0.2f).onComplete = () =>
                 {
                     Default.blocksRaycasts = false;
                     Dong.DOFade(1, 0.2f).onComplete = () =>
                     {
                         GameManager.Instance.EventCenter.SendEvent("�ֶ�����", new EventCenter.EventArgs() { Boolean = true });
                     };
                     Dong.blocksRaycasts = true;
                     Card.TruthUnable = false;
                     Card.InsertUnable = false;
                 };
            });
            Buttons[i].gameObject.SetActive(true);
            Buttons[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "> �ֶ�";
            ++i;
        }
        if (GameManager.Instance.EventCenter.TryGetEventArgs("�ö��������¼", out EventCenter.EventArgs eventArgs2) && eventArgs2.Boolean)
        {
            Buttons[i].onClick.AddListener(() =>
            {
                Default.DOFade(0, 0.2f).onComplete = () =>
                {
                    Default.blocksRaycasts = false;
                    Yue.DOFade(1, 0.2f).onComplete =()=>
                    {
                        GameManager.Instance.EventCenter.SendEvent("���㵵��", new EventCenter.EventArgs() { Boolean = true });
                        Card.TrueState = true;
                        Card.TruthUnable = false;
                        Card.InsertUnable = false;
                    };
                    Yue.blocksRaycasts = true;
                };
            });
            Buttons[i].gameObject.SetActive(true);
            Buttons[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "> ����";
            ++i;
        }

        if (i == 0)
        {
            GameManager.Instance.EventCenter.SendEvent("���ѵ���-û�е�����Ϣʱ��", new EventCenter.EventArgs() { Boolean = true });
        }

        if (HaveTag("��ǩֽ"))
        {
            MyButton.gameObject.SetActive(false);
        }

        MyButton.OnClick += MyButton_OnClick;
    }

    public bool HaveTag(string name)
    {
        return GameManager.Instance.EventCenter.TryGetEventArgs(name, out EventCenter.EventArgs eventArgs) && eventArgs.Boolean;
    }

    private void MyButton_OnClick()
    {
        GameManager.Instance.EventCenter.SendEvent("��ǩֽ", new EventCenter.EventArgs() { Boolean = true });
        if (HaveTag("�Ķ�������"))
        {
            GameManager.Instance.EventCenter.SendEvent("��ǩֽ-����Ķ�������", new EventCenter.EventArgs() { Boolean = true });
        }
        else
        {
            GameManager.Instance.EventCenter.SendEvent("��ǩֽ-û���������", new EventCenter.EventArgs() { Boolean = true });
        }
        MyButton.gameObject.SetActive(false);
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }
}
