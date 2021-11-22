using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Linq;
using System;

public class TalkSystem : SerializedMonoBehaviour
{
    /// <summary>
    /// 段落
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
        [LabelText("角色名")]
        public string Name;
        [LabelText("正文"), Multiline]
        public string Body;
        [LabelText("游戏物体名")]
        public string ObjectName;
        [LabelText("是否播放完立即跳转至下一对话")]
        public bool Skip;
        [LabelText("对话框类型"), EnumToggleButtons]
        public DialogType Dialog;
        [LabelText("操作类型"), EnumToggleButtons]
        public ControlType Control;

    }

    public static TalkSystem Instance
    {
        get;
        private set;
    }

    public GameObject BubbleDialogPrefabs;

    [SerializeField]
    private Queue<TextBody> _textBodies = new Queue<TextBody>();

    [SerializeField]
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

    [SerializeField]
    public Dialogue.DialogueTree DialogueTree
    {
        get => _dialogueTree;
        set
        {
            if (value)
            {
                _dialogueTree = value.Clone() as Dialogue.DialogueTree;
            }
            else
            {
                _dialogueTree = null;
            }
        }
    }
    private Dialogue.DialogueTree _dialogueTree;

    private void Start()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        GameManager.Instance.EventCenter.AddListener("DIALOG_CLOSE", e => _dialog.gameObject.SetActive(false));
        GameManager.Instance.EventCenter.AddListener("DIALOG_PUSH", e => PushBodies(e.Object as TextBody[]));
    }

    private void Update()
    {
        if (_textBodies.Count > 0)
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
        }
        if (DialogueTree)
        {
            if(DialogueTree.Tick(null) == NodeStatus.Success)
            {
                Debug.Log(2);
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
            dialog.BeginRead(body.Body);
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
