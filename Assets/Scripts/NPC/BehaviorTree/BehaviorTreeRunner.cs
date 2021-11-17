using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace NPC
{
    public class BehaviorTreeRunner : SerializedMonoBehaviour
    {
        public BehaviorTree BehaviorTree;

        /// <summary>
        /// ������
        /// </summary>
        public Dictionary<string, EventCenter.EventArgs> Variables;

        private void Awake()
        {
            BehaviorTree = BehaviorTree.Clone();
        }

        private void Update()
        {
            BehaviorTree.Tick(this);
        }
    }
}
