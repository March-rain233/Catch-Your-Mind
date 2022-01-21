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
    /// 事件的执行树
    /// </summary>
    public class ExecutorNode : Node, ITree
    {
        public INode RootNode => _root;

        public Type NodeParentType => typeof(ActionNode);

        public Type RootType => typeof(ActionRoot);

        [SerializeField]
        private List<ActionNode> _actionNodes = new List<ActionNode>();

        [SerializeField]
        private ActionRoot _root;

        protected override void EventHandler(string eventName, EventCenter.EventArgs eventArgs)
        {
            base.EventHandler(eventName, eventArgs);
            _root.Tick(eventName, eventArgs);
        }

        public void AddNode(INode node)
        {
            _actionNodes.Add(node as ActionNode);
        }

        public void ConnectNode(INode parent, INode child)
        {
            (parent as ActionNode).Nodes.Add(child as ActionNode);
        }

        public INode CreateNode(Type type)
        {

            var node = CreateInstance(type) as ActionNode;
            node.name = type.Name;
            _actionNodes.Add(node);
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
            (parent as ActionNode).Nodes.Remove(child as ActionNode);
        }

        public INode[] GetNodes()
        {
            return _actionNodes.ToArray();
        }

        public void RemoveNode(INode node)
        {
            var toRemove = node as ActionNode;
            _actionNodes.Remove(toRemove);
            _actionNodes.ForEach(node =>
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
            _root = CreateNode(typeof(ActionRoot)) as ActionRoot;
        }
    }
}
