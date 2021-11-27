using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

namespace NPC
{
    /// <summary>
    /// ��Ϊ��
    /// </summary>
    [CreateAssetMenu(fileName = "��Ϊ��������", menuName = "��ɫ/��Ϊ��������")]
    public class BehaviorTree : ScriptableObject, ITree, IRunable
    {
        /// <summary>
        /// ���ڵ�
        /// </summary>
        [SerializeField]
        private RootNode _rootNode;

        /// <summary>
        /// �ڵ��б�
        /// </summary>
        [SerializeField]
        public List<Node> Nodes = new List<Node>();

        public INode RootNode => _rootNode;

        public Type NodeParentType => typeof(Node);

        public Type RootType => typeof(RootNode);

        /// <summary>
        /// ����
        /// </summary>
        /// <param name="runner"></param>
        /// <returns></returns>
        public NodeStatus Tick(BehaviorTreeRunner runner)
        {
            if (_rootNode.Status == NodeStatus.Running)
            {
                _rootNode.Tick(runner);
            }
            return _rootNode.Status;
        }

        /// <summary>
        /// ��ӽڵ�
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public Node AddNode(Type type)
        {
            var node = CreateInstance(type) as Node;

            int count = Nodes.FindAll(node => node.Name == type.Name).Count;
            string newName = type.Name;
            if (count > 0)
            {
                newName = newName + $"({count})";
            }
            node.name = newName;

            node.Guid = GUID.Generate().ToString();
            Nodes.Add(node);

            if (AssetDatabase.Contains(this))
            {
                AssetDatabase.AddObjectToAsset(node, this);
                AssetDatabase.SaveAssets();
            }
            return node;
        }

        /// <summary>
        /// �Ƴ��ڵ�
        /// </summary>
        /// <param name="node"></param>
        public void RemoveNode(Node node)
        {
            Nodes.Remove(node);

            //�Ƴ��븸�ڵ������
            Node parent = null;
            for(int i = 0; i < Nodes.Count; ++i)
            {
                if(Nodes[i] == node) { continue; }
                parent = FindParent(node, Nodes[i]);
                if(parent != null) { break; }
            }
            DisconnectNode(parent, node);

            if (AssetDatabase.Contains(this))
            {
                AssetDatabase.RemoveObjectFromAsset(node);
                AssetDatabase.SaveAssets();
            }
        }

        /// <summary>
        /// Ѱ��ָ���ڵ�ĸ��ڵ�
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public Node FindParent(Node target, Node start)
        {
            if (target == start) { return start; }
            foreach(var child in start.GetChildren())
            {
                if(child as Node == target)
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

        /// <summary>
        /// ��¡
        /// </summary>
        /// <returns></returns>
        public IRunable Clone()
        {
            var tree = Instantiate(this);
            tree._rootNode = _rootNode.Clone() as RootNode;
            Stack<Node> stack = new Stack<Node>(Nodes.Count);
            stack.Push(tree._rootNode);
            tree.Nodes.Clear();
            while (stack.Count > 0)
            {
                var node = stack.Pop();
                Array.ForEach(node.GetChildren(), child => stack.Push(child as Node));
                tree.Nodes.Add(node);
            }
            return tree;
        }

        public INode[] GetNodes()
        {
            return Nodes.ToArray();
        }

        public INode CreateNode(Type type)
        {
            return AddNode(type);
        }

        public void RemoveNode(INode node)
        {
            RemoveNode(node as Node);
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
                var node = parent as DecoratorNode;
                if (node != null)
                {
                    node.Child = child as Node;
                    return;
                }
            }

            Debug.LogError("�Ƿ���Ҷ�ӽڵ㵱�����ڵ�");
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
                var node = parent as DecoratorNode;
                if (node != null)
                {
                    node.Child = null;
                    return;
                }
            }

            //Debug.LogError(parent == null);
            //Debug.LogError("�Ƿ���Ҷ�ӽڵ㵱�����ڵ�");
        }

        public void SetRoot()
        {
            _rootNode = AddNode(typeof(RootNode)) as RootNode;
            RootNode.GetType().GetField("_status").SetValue(_rootNode, NodeStatus.Running);
        }

        public void AddNode(INode node)
        {
            Nodes.Add(node as Node);
        }
    }
}
