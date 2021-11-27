using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class StatusPanel :MonoBehaviour
{
    public TextMeshProUGUI InteractionText;

    public FillSlider TimeView;

    private void Start()
    {
        InteractionText.text = "";
        InteractionText.DOFade(0, 0);
        GameManager.Instance.EventCenter.AddListener("InteractionEnter", InteractionEnter);
        GameManager.Instance.EventCenter.AddListener("InteractionExit", InteractionExit);
        GameManager.Instance.TimeChanged += time =>
        {
            TimeView.Value = time / GameManager.Instance.MaxTime;
        };
    }

    private void InteractionEnter(EventCenter.EventArgs eventArgs)
    {
        InteractionText.DOFade(1, 0.5f);
        InteractionText.text = eventArgs.String;
    }

    private void InteractionExit(EventCenter.EventArgs eventArgs)
    {
        InteractionText.DOFade(0, 0.5f);
    }
}
