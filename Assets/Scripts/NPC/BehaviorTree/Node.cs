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
        /// ��ǰ״̬
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

        public abstract UnityEditor.Experimental.GraphView.Port.Capacity Output { get; }

        public virtual bool IsRoot => false;

        public virtual bool IsLeaf => false;

        public UnityEditor.Experimental.GraphView.Port.Capacity Input => UnityEditor.Experimental.GraphView.Port.Capacity.Single;

        private NodeStatus _status = NodeStatus.Success;

        public event Action<string> OnNameChanged;
        public event Action<Color> OnStatusChanged;

        private bool _isStart = false;

        /// <summary>
        /// �����ø���
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
            //Debug.Log("��������" + name + _status.ToString());
            if (Status != NodeStatus.Running)
            {
                OnExit(runner);
                _isStart = false;
            }

            return Status;
        }

        /// <summary>
        /// ����ʱ
        /// </summary>
        protected abstract void OnEnter(BehaviorTreeRunner runner);

        /// <summary>
        /// �˳�ʱ
        /// </summary>
        protected abstract void OnExit(BehaviorTreeRunner runner);

        /// <summary>
        /// ���ʱ
        /// </summary>
        /// <param name="runner"></param>
        protected abstract void OnAbort(BehaviorTreeRunner runner);

        /// <summary>
        /// �ָ�ʱ
        /// </summary>
        /// <param name="runner"></param>
        protected abstract void OnResume(BehaviorTreeRunner runner);

        /// <summary>
        /// �����ô��
        /// </summary>
        /// <param name="runner"></param>
        public void Abort(BehaviorTreeRunner runner)
        {
            if (Status == NodeStatus.Aborting) { return; }
            Status = NodeStatus.Aborting;
            OnAbort(runner);
        }

        /// <summary>
        /// ����ʱ
        /// </summary>
        protected abstract NodeStatus OnUpdate(BehaviorTreeRunner runner);

        public virtual Node Clone(bool self = false)
        {
            var node = Instantiate(this);
            node.ViewPosition = ViewPosition;
            node.Guid = GUID.Generate().ToString();
            return node;
        }

        public abstract INode[] GetChildren();

        [Button("�޸Ľڵ���")]
        public void ChangeName(string newName) 
        {
            name = newName;
            OnNameChanged?.Invoke(name);
        }

        [Button("����GUID")]
        public void ChangeGuid()
        {
            Guid = GUID.Generate().ToString();
        }

        [Button("���浽")]
        private void SaveAs()
        {
            string path = EditorUtility.SaveFilePanelInProject("����Ϊ", name, "asset", "һ����û�뵽�Ĵ���������Ҿܲ�����");
            Node clone = Clone();
            AssetDatabase.CreateAsset(clone, path);
            Stack<Node> stack = new Stack<Node>();
            stack.Push(clone);
            while (stack.Count > 0)
            {
                var node = stack.Pop();
                Array.ForEach(node.GetChildren(), child => stack.Push(child as Node));
                node.name = node.name.Replace("(Clone)", "");
                if(node != clone)
                {
                    AssetDatabase.AddObjectToAsset(node, clone);
                }
            }
        }
    }
}
