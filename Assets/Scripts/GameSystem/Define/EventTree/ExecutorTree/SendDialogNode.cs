using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.Serialization;

namespace EventTree
{

    public class SendDialogNode : ActionNode
    {
        [System.Serializable]
        private struct Dialog
        {
            public Dialogue.DialogueTree DialogueTree;
            public bool JustOnce;
        }
        [OdinSerialize]
        private Dictionary<string, Dialog> _dialogs;

        protected override void EventHandler(string eventName, EventCenter.EventArgs eventArgs)
        {
            if (_dialogs.ContainsKey(eventName))
            {
                var dialog = _dialogs[eventName];
                if (GameManager.Instance.GameSave.Story.HaveTriggered.Contains(eventName) && dialog.JustOnce)
                {
                    return;
                }
                if (!GameManager.Instance.GameSave.Story.HaveTriggered.Contains(eventName)) 
                { GameManager.Instance.GameSave.Story.HaveTriggered.Add(eventName); }
                TalkSystem.Instance.DialogueTree = dialog.DialogueTree;
            }
        }
    }
}
