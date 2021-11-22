using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Dialogue
{
    /// <summary>
    /// ��������ڵ�
    /// </summary>
    public class DialogueNode : ActionNode
    {
        /// <summary>
        /// ������
        /// </summary>
        [SerializeField]
        private TalkSystem.TextBody[] _paragraphs;

        private bool _readEnd;

        protected override void OnEnter(DialogueTree tree)
        {
            base.OnEnter(tree);
            _readEnd = false;
            GameManager.Instance.EventCenter.AddListener("DIALOG_END", DialogEndHandler);
            GameManager.Instance.EventCenter.SendEvent("DIALOG_PUSH", new EventCenter.EventArgs() { Object = _paragraphs });
        }

        protected override NodeStatus OnUpdate(DialogueTree tree)
        {
            if (_readEnd) { return NodeStatus.Success; }
            return NodeStatus.Running;
        }

        private void DialogEndHandler(EventCenter.EventArgs e) { _readEnd = true; }

        protected override void OnExit(DialogueTree tree)
        {
            base.OnExit(tree);
            GameManager.Instance.EventCenter.RemoveListener("DIALOG_END", DialogEndHandler);
        }
    }
}
