using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEditor;

namespace Dialogue
{
    public abstract class Node : ScriptableObject, INode
    {
        [SerializeField, HideInInspector]
        public string Guid { get; set; }

        public string Name => name;

        public Vector2 ViewPosition { get; set; }

        public virtual bool IsRoot => false;

        public virtual bool IsLeaf => false;

        public virtual Port.Capacity Input => Port.Capacity.Multi;

        public virtual Port.Capacity Output => Port.Capacity.Single;

        private bool _isStarted = false;

        /// <summary>
        /// 进入该节点的条件
        /// </summary>
        [SerializeField]
        private Condition _condition;

        public event Action<string> OnNameChanged;
        public event Action<NodeStatus> OnStatusChanged;

        public abstract INode[] GetChildren();

        /// <summary>
        /// 判断进入该节点的条件是否成立
        /// </summary>
        /// <param name="tree"></param>
        /// <returns></returns>
        public bool JudgeCondition(DialogueTree tree)
        {
            if (_condition == null) { return true; }
            return _condition.Reason(tree);
        }

        public virtual Node Tick(DialogueTree tree)
        {
            if (!_isStarted)
            {
                _isStarted = true;
                OnEnter(tree);
            }
            var status = OnUpdate(tree);
            if (status == NodeStatus.Running)
            {
                return this;
            }

            _isStarted = false;
            OnExit(tree);
            return SelectChild(tree);
        }

        protected virtual void OnEnter(DialogueTree tree) { }

        protected virtual void OnExit(DialogueTree tree) { }

        protected abstract NodeStatus OnUpdate(DialogueTree tree);

        protected abstract Node SelectChild(DialogueTree tree);

        public virtual Node Clone()
        {
            var node = Instantiate(this);
            node.ViewPosition = ViewPosition;
            node.Guid = GUID.Generate().ToString();
            return node;
        }

        [Button("修改节点名")]
        public void ChangeName(string newName)
        {
            name = newName;
            OnNameChanged?.Invoke(name);
        }

        [Button("更改GUID")]
        public void ChangeGuid()
        {
            Guid = GUID.Generate().ToString();
        }
    }
}
