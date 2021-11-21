using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Linq;
using System;

public class TalkSystem : MonoBehaviour
{
    /// <summary>
    /// ����
    /// </summary>
    [System.Serializable]
    public struct TextBody
    {
        [System.Serializable]
        public enum DialogType
        {
            Bubble,
            Queue,
        }
        [System.Serializable]
        public enum ControlType
        {
            KeyBoard,
            Mouse,
            Both
        }
        [LabelText("��ɫ��")]
        public string Name;
        [LabelText("����"), Multiline]
        public string Body;
        [LabelText("��Ϸ������")]
        public string ObjectName;
        [LabelText("�Ƿ񲥷���������ת����һ�Ի�")]
        public bool Skip;
        [LabelText("�Ի�������"), EnumToggleButtons]
        public DialogType Dialog;
        [LabelText("��������"), EnumToggleButtons]
        public ControlType Control;

    }

    public static TalkSystem Instance
    {
        get;
        private set;
    }

    public GameObject BubbleDialogPrefabs;

    private Queue<TextBody> _textBodies = new Queue<TextBody>();

    private bool _skip = false;

    private BubbleDialog BubbleDialog
    {
        get
        {
            if (_dialog == null)
            {
                _dialog = Instantiate(BubbleDialogPrefabs, GameObject.Find("UIroot").transform).GetComponent<BubbleDialog>();
                _dialog.ShowEnd += TextEnd;
            }
            return _dialog;
        }
    }
    [SerializeField]
    private BubbleDialog _dialog;

    public Dialogue.DialogueTree DialogueTree;

    private void Start()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        GameManager.Instance.EventCenter.AddListener("NextDialog", e => NextStep());
        GameManager.Instance.EventCenter.AddListener("CloseDialog", e => _dialog.gameObject.SetActive(false));
        GameManager.Instance.EventCenter.AddListener("ReadBodies", e => PushBodies(e.Object as TextBody[]));
    }

    private void Update()
    {
        bool interact = Input.GetKeyDown(GameManager.Instance.ControlManager.KeyDic[KeyType.Interact]);
        bool skip = Input.GetKey(GameManager.Instance.ControlManager.KeyDic[KeyType.Skip]);
        bool mouse = Input.GetMouseButtonDown(0);
        bool next = false;
        switch (_textBodies.Peek().Control)
        {
            case TextBody.ControlType.KeyBoard:
                next = skip | interact;
                break;
            case TextBody.ControlType.Mouse:
                next = mouse;
                break;
            case TextBody.ControlType.Both:
                next = mouse | interact | skip;
                break;
        }
        if (next) { NextStep(); }
        if (DialogueTree)
        {
            if(DialogueTree.Tick(null) == NodeStatus.Success)
            {
                DialogueTree = null;
            }
        }
    }

    public void PushBodies(TextBody[] textBodies)
    {
        Array.ForEach(textBodies, body => _textBodies.Enqueue(body));
        NextStep();
    }

    public void NextStep()
    {
        if (_textBodies.Count <= 0) 
        { 
            GameManager.Instance.EventCenter.SendEvent("DIALOG_END", new EventCenter.EventArgs());
            return;
        }
        var body = _textBodies.Peek();
        switch (body.Dialog)
        {
            case TextBody.DialogType.Bubble:
                BubbleDialogHandler(body);
                break;
            case TextBody.DialogType.Queue:
                QueueDialogHandler(body);
                break;
        }
    }

    private void TextEnd()
    {
        _textBodies.Dequeue();
        if (_skip) { NextStep(); }
    }

    private void QueueDialogHandler(TextBody body)
    {
        var queue = Transform.FindObjectOfType<MakerPanel>();
        var dialog = queue.GetCurrentDialog();
        if(dialog && dialog.Typing)
        {
            dialog.OutputImmediately();
        }
        else
        {
            _skip = body.Skip;
            dialog = queue.EnqueueDialog(body);
            dialog.ShowEnd += TextEnd;
            switch (body.Name)
            {
                case "Mind":
                    dialog.HeadIcon = StaticDialog.Person.Mind;
                    break;
                case "Emo":
                    dialog.HeadIcon = StaticDialog.Person.Emo;
                    break;
                default:
                    dialog.HeadIcon = StaticDialog.Person.None;
                    break;
            }
        }
    }

    private void BubbleDialogHandler(TextBody body)
    {
        if (!BubbleDialog.isActiveAndEnabled) { BubbleDialog.gameObject.SetActive(true); }
        if (BubbleDialog.Typing)
        {
            BubbleDialog.OutputImmediately();
        }
        else
        {
            if (!string.IsNullOrEmpty(body.Name))
            {
                BubbleDialog.SetName(body.Name);
                if (string.IsNullOrEmpty(body.ObjectName))
                {
                    SetDialogFollow(GameObject.Find(body.Name).transform);
                }
            }
            if (!string.IsNullOrEmpty(body.ObjectName))
            {
                SetDialogFollow(GameObject.Find(body.ObjectName).transform);
            }
            _skip = body.Skip;
            BubbleDialog.BeginRead(body.Body);
        }
    }

    private void SetDialogFollow(Transform follow)
    {
        BubbleDialog.Follow = follow;
    }
}
