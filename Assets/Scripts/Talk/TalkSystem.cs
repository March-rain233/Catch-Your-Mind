using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Linq;

public class TalkSystem : MonoBehaviour
{
    public static TalkSystem Instance
    {
        get;
        private set;
    }
    /// <summary>
    /// 段落
    /// </summary>
    [System.Serializable]
    public struct TextBody
    {
        [LabelText("角色名")]
        public string Name;
        [LabelText("正文"), Multiline]
        public string Body;
        [LabelText("游戏物体名")]
        public string ObjectName;
        [LabelText("是否播放完立即跳转至下一对话")]
        public bool Skip;
    }

    public GameObject DialogPrefabs;

    private Queue<TextBody> _textBodies = new Queue<TextBody>();

    private bool _skip = false;

    private BubbleDialog Dialog
    {
        get
        {
            if (_dialog == null)
            {
                _dialog = Instantiate(DialogPrefabs, GameObject.Find("UIroot").transform).GetComponent<BubbleDialog>();
                _dialog.ShowEnd += () =>
                {
                    if (_skip) { NextStep(); }
                };
            }
            return _dialog;
        }
    }
    [SerializeField]
    private BubbleDialog _dialog;

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

    public void PushBodies(TextBody[] textBodies)
    {
        System.Array.ForEach(textBodies, body => _textBodies.Enqueue(body));
        NextStep();
    }

    public void NextStep()
    {
        if (_textBodies.Count <= 0) 
        { 
            GameManager.Instance.EventCenter.SendEvent("DialogEnd", new EventCenter.EventArgs());
            return;
        }
        if (!Dialog.isActiveAndEnabled) { Dialog.gameObject.SetActive(true); }
        if (Dialog.Typing)
        {
            Dialog.OutputImmediately();
        }
        else
        {
            SolveBody(_textBodies.Dequeue());
        }
    }

    private void SolveBody(TextBody body)
    {
        if (!string.IsNullOrEmpty(body.Name))
        {
            Dialog.SetName(body.Name);
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
        Dialog.BeginRead(body.Body);
    }

    private void SetDialogFollow(Transform follow)
    {
        Dialog.Follow = follow;
    }
}
