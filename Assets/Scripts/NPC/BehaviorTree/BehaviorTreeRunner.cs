using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace NPC
{
    public class BehaviorTreeRunner : SerializedMonoBehaviour
    {
        public IRunable BehaviorTree;

        /// <summary>
        /// ╠Да©©Б
        /// </summary>
        public Dictionary<string, EventCenter.EventArgs> Variables;

        private bool _stop = false;

        private void Awake()
        {
            BehaviorTree = BehaviorTree.Clone();
            GameManager.Instance.EventCenter.AddListener("DIALOG_ENTER", EnterDialogue);
            GameManager.Instance.EventCenter.AddListener("DIALOG_EXIT", ExitDialogue);
        }

        private void Update()
        {
            if (_stop) return;
            BehaviorTree.Tick(this);
        }

        private void EnterDialogue(EventCenter.EventArgs e)
        {
            _stop = true;
        }

        private void ExitDialogue(EventCenter.EventArgs e)
        {
            _stop = false;
        }
    }
}
