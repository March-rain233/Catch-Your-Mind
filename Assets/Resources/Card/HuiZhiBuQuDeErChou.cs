using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class HuiZhiBuQuDeErChou : MonoBehaviour
{
    public Image Image;
    public MyButton Button;
    public TextMeshProUGUI TextMeshPro;

    int time = 0;

    private void Awake()
    {
        Button.OnClick += Click;
        Image.DOFade(0, 0);
    }

    public void Click()
    {
        ++time;
        if (time == 5)
        {
            TextMeshPro.text = ">   【再点就真的显示了】";
        }
        if(time == 6)
        {
            Button.gameObject.SetActive(false);
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
            Image.DOFade(1, 0.5f);
        }
    }

}
