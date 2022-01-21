using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace EventTree
{
    /// <summary>
    /// 系统事件响应树
    /// </summary>
    /// <remarks>
    /// 对事件中心发出的事件做系统调度上的回应
    /// </remarks>
    [CreateAssetMenu(fileName = "事件响应树", menuName = "系统/事件树")]
    public class EventTree : ScriptableObject, ITree
    {
        public INode RootNode => _root;

        public Type NodeParentType => typeof(Node);

        public Type RootType => typeof(RootNode);

        [SerializeField]
        private RootNode _root;

        [SerializeField]
        private List<Node> _nodes = new List<Node>();

        /// <summary>
        /// 初始化
        /// </summary>
        public void Init()
        {
            GameManager.Instance.EventCenter.EventChanged += EventHandler;
        }

        public void EventHandler(string eventName, EventCenter.EventArgs eventArgs)
        {
            _root.Tick(eventName, eventArgs);
        }

        public void AddNode(INode node)
        {
            _nodes.Add(node as Node);
        }

        public void ConnectNode(INode parent, INode child)
        {
            (parent as Node).Nodes.Add(child as Node);
        }

        public INode CreateNode(Type type)
        {
            var node = CreateInstance(type) as Node;
            node.name = type.Name;
            _nodes.Add(node);
#if UNITY_EDITOR
            node.Guid = GUID.Generate().ToString();
            if (AssetDatabase.Contains(this))
            {
                AssetDatabase.AddObjectToAsset(node, this);
                AssetDatabase.SaveAssets();
            }
#endif
            return node;
        }

        public void DisconnectNode(INode parent, INode child)
        {
            (parent as Node).Nodes.Remove(child as Node);
        }

        public INode[] GetNodes()
        {
            return _nodes.ToArray();
        }

        public void RemoveNode(INode node)
        {
            var toRemove = node as Node;
            _nodes.Remove(toRemove);
            _nodes.ForEach(node =>
            {
                node.Nodes.Remove(toRemove);
            });
#if UNITY_EDITOR
            if (AssetDatabase.Contains(this))
            {
                AssetDatabase.RemoveObjectFromAsset(toRemove);
                AssetDatabase.SaveAssets();
            }
#endif
        }

        public void SetRoot()
        {
            _root = CreateNode(typeof(RootNode)) as RootNode;
        }
    }
}
