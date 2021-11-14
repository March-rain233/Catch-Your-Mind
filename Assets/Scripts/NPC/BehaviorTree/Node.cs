using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEditor;
using System;

namespace NPC
{

    public abstract class Node : SerializedScriptableObject, INode
    {
        [OdinSerialize, ReadOnly]
        public string Guid { get; set; }

        /// <summary>
        /// 节点状态
        /// </summary>
        [System.Serializable]
        public enum NodeStatus
        {
            Success,
            Failure,
            Running,
            Aborting,
        }

        /// <summary>
        /// 当前状态
        /// </summary>
        public NodeStatus Status
        {
            get => _status;
            private set
            {
                _status = value;
                OnStatusChanged?.Invoke(_status);
            }
        }

        public string Name => name;

        public Vector2 ViewPosition { get; set; }

        public abstract UnityEditor.Experimental.GraphView.Port.Capacity Output { get; }

        public virtual bool IsRoot => false;

        public virtual bool IsLeaf => false;

        public UnityEditor.Experimental.GraphView.Port.Capacity Input => UnityEditor.Experimental.GraphView.Port.Capacity.Single;

        [SerializeField, HideInInspector, SetProperty("Status")]
        private NodeStatus _status = NodeStatus.Success;

        public event Action<string> OnNameChanged;
        public event Action<NodeStatus> OnStatusChanged;

        private bool _isStart = false;

        /// <summary>
        /// 外界调用更新
        /// </summary>
        /// <returns></returns>
        public NodeStatus Tick(BehaviorTreeRunner runner)
        {
            if (!_isStart)
            {
                OnEnter(runner);
                _isStart = true;
            }
            if (Status == NodeStatus.Aborting)
            {
                Status = NodeStatus.Running;
                OnResume(runner);
            }
            Status  = OnUpdate(runner);
            //Debug.Log("正在运行" + name + _status.ToString());
            if (Status != NodeStatus.Running)
            {
                OnExit(runner);
                _isStart = false;
            }

            return Status;
        }

        /// <summary>
        /// 进入时
        /// </summary>
        protected abstract void OnEnter(BehaviorTreeRunner runner);

        /// <summary>
        /// 退出时
        /// </summary>
        protected abstract void OnExit(BehaviorTreeRunner runner);

        /// <summary>
        /// 打断时
        /// </summary>
        /// <param name="runner"></param>
        protected abstract void OnAbort(BehaviorTreeRunner runner);

        /// <summary>
        /// 恢复时
        /// </summary>
        /// <param name="runner"></param>
        protected abstract void OnResume(BehaviorTreeRunner runner);

        /// <summary>
        /// 外界调用打断
        /// </summary>
        /// <param name="runner"></param>
        public void Abort(BehaviorTreeRunner runner)
        {
            if (Status == NodeStatus.Aborting) { return; }
            Status = NodeStatus.Aborting;
            OnAbort(runner);
        }

        /// <summary>
        /// 更新时
        /// </summary>
        protected abstract NodeStatus OnUpdate(BehaviorTreeRunner runner);

        public virtual Node Clone()
        {
            var node = Instantiate(this);
            node.ViewPosition = ViewPosition;
            return node;
        }

        public abstract INode[] GetChildren();

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
