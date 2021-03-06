using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System;

namespace NPC
{

    public abstract class Node : SerializedScriptableObject, INode
    {
        [OdinSerialize, ReadOnly]
        public string Guid { get; set; }

        /// <summary>
        /// 当前状态
        /// </summary>
        [OdinSerialize]
        public NodeStatus Status
        {
            get => _status;
            private set
            {
                _status = value;
                switch (_status)
                {
                    case NodeStatus.Success:
                        OnStatusChanged?.Invoke(Color.green);
                        break;
                    case NodeStatus.Failure:
                        OnStatusChanged?.Invoke(Color.red);
                        break;
                    case NodeStatus.Running:
                        OnStatusChanged?.Invoke(Color.blue);
                        break;
                    case NodeStatus.Aborting:
                        OnStatusChanged?.Invoke(Color.yellow);
                        break;
                }
            }
        }

        public string Name => name;

        public Vector2 ViewPosition { get; set; }

#if UNITY_EDITOR
        public abstract UnityEditor.Experimental.GraphView.Port.Capacity Output { get; }

        public UnityEditor.Experimental.GraphView.Port.Capacity Input => UnityEditor.Experimental.GraphView.Port.Capacity.Single;
#endif

        public virtual bool IsRoot => false;

        public virtual bool IsLeaf => false;

        private NodeStatus _status = NodeStatus.Success;

        public event Action<string> OnNameChanged;
        public event Action<Color> OnStatusChanged;

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

        public virtual Node Clone(bool self = false)
        {
            var node = Instantiate(this);
            node.ViewPosition = ViewPosition;
#if UNITY_EDITOR
            node.Guid = GUID.Generate().ToString();
#endif
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
#if UNITY_EDITOR
            Guid = GUID.Generate().ToString();
#endif
        }

        [Button("保存到")]
        private void SaveAs()
        {
#if UNITY_EDITOR
            string path = EditorUtility.SaveFilePanelInProject("保存为", name, "asset", "一切我没想到的错误操作，我拒不负责！");
            Node clone = Clone();
            AssetDatabase.CreateAsset(clone, path);
            Stack<Node> stack = new Stack<Node>();
            stack.Push(clone);
            while (stack.Count > 0)
            {
                var node = stack.Pop();
                Array.ForEach(node.GetChildren(), child => stack.Push(child as Node));
                node.name = node.name.Replace("(Clone)", "");
                if (node != clone)
                {
                    AssetDatabase.AddObjectToAsset(node, clone);
                }
            }
#endif
        }
    }
}
