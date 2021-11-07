using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusPanel : BasePanel
{
    public override PanelType Type => PanelType.StatusPanel;

    public Text InteractionText;

    private void Start()
    {
        GameManager.Instance.EventCenter.AddListener("InteractionEnter", InteractionEnter);
        GameManager.Instance.EventCenter.AddListener("InteractionExit", InteractionExit);
    }

    private void InteractionEnter(EventCenter.EventArgs eventArgs)
    {
        _animator.SetTrigger("InteractionEnter");
        InteractionText.text = "°´ÏÂ" + (eventArgs.Object as InteractionTrigger).InteractKey
            + "À´" + eventArgs.String;
    }

    private void InteractionExit(EventCenter.EventArgs eventArgs)
    {
        _animator.SetTrigger("InteractionExit");
        Debug.Log(1);
    }

    public override void NotifyHandler(string name, object value)
    {
        throw new System.NotImplementedException();
    }

    protected override void OnExit()
    {
        throw new System.NotImplementedException();
    }

    protected override void OnPause()
    {
        throw new System.NotImplementedException();
    }

    protected override void OnResume()
    {
        throw new System.NotImplementedException();
    }

    protected internal override void Init()
    {

    }

    protected internal override void OnEnter()
    {
        throw new System.NotImplementedException();
    }
}
