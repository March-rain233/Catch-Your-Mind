using NPC;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Dialogue
{
    /// <summary>
    /// 对话树
    /// </summary>
    [CreateAssetMenu(fileName = "对话树", menuName = "对话/对话树")]
    public class DialogueTree : NPC.ActionNode, ITree
    {
        public INode RootNode => _rootNode;

        public Type NodeParentType => typeof(Dialogue.Node);

        public Type RootType => typeof(Dialogue.RootNode);

        /// <summary>
        /// 根节点
        /// </summary>
        [SerializeField]
        private RootNode _rootNode;

        /// <summary>
        /// 当前运行节点
        /// </summary>
        [SerializeField]
        private Node _currentNode;

        [SerializeField]
        private List<Node> _nodes = new List<Node>();

        public Dictionary<string, EventCenter.EventArgs> Variables;

        protected override NodeStatus OnUpdate(BehaviorTreeRunner runner)
        {
            _currentNode = _currentNode.Tick(this);
            if (_currentNode != null)
            {
                return NodeStatus.Running;
            }
            GameManager.Instance.EventCenter.SendEvent("DIALOG_EXIT", new EventCenter.EventArgs());
            return NodeStatus.Success;
        }

        public override NPC.Node Clone(bool self = false)
        {
            var tree = Instantiate(this);
            tree.name = tree.name.Replace("(Clone)", "") + "(Clone)";
            tree._rootNode = _rootNode.Clone() as RootNode;
            Stack<Node> stack = new Stack<Node>(_nodes.Count);
            stack.Push(tree._rootNode);
            tree._nodes.Clear();
            while (stack.Count > 0)
            {
                var node = stack.Pop();
                Array.ForEach(node.GetChildren(), child => stack.Push(child as Node));
                tree._nodes.Add(node);
            }
            if(tree.Variables == null)
            {
                tree.Variables = new Dictionary<string, EventCenter.EventArgs>();
            }
            return tree;
        }

        public void ConnectNode(INode parent, INode child)
        {
            {
                var node = parent as CompositeNode;
                if (node != null)
                {
                    node.Childrens.Add(child as Node);
                    return;
                }
            }
            {
                var node = parent as ActionNode;
                if (node != null)
                {
                    node.Child = child as Node;
                    return;
                }
            }
        }

        public INode CreateNode(Type type)
        {
            var node = CreateInstance(type) as Node;
            node.name = type.Name;
            node.Guid = GUID.Generate().ToString();
            _nodes.Add(node);

            if (AssetDatabase.Contains(this))
            {
                AssetDatabase.AddObjectToAsset(node, this);
                AssetDatabase.SaveAssets();
            }
            return node;
        }

        public void DisconnectNode(INode parent, INode child) 
        { 
            {
                var node = parent as CompositeNode;
                if (node != null)
                {
                    node.Childrens.Remove(child as Node);
                    return;
                }
            }
            {
                var node = parent as ActionNode;
                if (node != null)
                {
                    node.Child = null;
                    return;
                }
            }
        }

        public INode[] GetNodes()
        {
            return _nodes.ToArray();
        }

        public void RemoveNode(INode nodeToRemove)
        {
            Node node = nodeToRemove as Node;
            _nodes.Remove(node);

            //移除与父节点的连接
            Node parent = null;
            for (int i = 0; i < _nodes.Count; ++i)
            {
                if (_nodes[i] == node) { continue; }
                parent = FindParent(node, _nodes[i]);
                if (parent != null) { break; }
            }
            DisconnectNode(parent, node);

            if (AssetDatabase.Contains(this))
            {
                AssetDatabase.RemoveObjectFromAsset(node);
                AssetDatabase.SaveAssets();
            }
        }

        /// <summary>
        /// 寻找指定节点的父节点
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public Node FindParent(Node target, Node start)
        {
            if (target == start) { return start; }
            foreach (var child in start.GetChildren())
            {
                if (child as Node == target)
                {
                    return start;
                }
                else
                {
                    var p = FindParent(target, child as Node);
                    if (p != null)
                    {
                        return p;
                    }
                }
            }
            return null;
        }

        public void SetRoot()
        {
            _rootNode = CreateNode(typeof(RootNode)) as RootNode;
            //RootNode.GetType().GetField("_status").SetValue(_rootNode, NodeStatus.Success);
        }

        protected override void OnAbort(BehaviorTreeRunner runner)
        {
            
        }

        protected override void OnEnter(BehaviorTreeRunner runner)
        {
            _currentNode = _rootNode;
            GameManager.Instance.EventCenter.SendEvent("DIALOG_ENTER", new EventCenter.EventArgs());
        }

        protected override void OnExit(BehaviorTreeRunner runner)
        {
            
        }

        protected override void OnResume(BehaviorTreeRunner runner)
        {
            
        }

        public void AddNode(INode node)
        {
            _nodes.Add(node as Node);
        }
    }
}
