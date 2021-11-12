using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

/// <summary>
/// 行为树
/// </summary>
[CreateAssetMenu(fileName ="行为树控制器", menuName = "角色/行为树控制器")]
public class BehaviorTree : ScriptableObject, ITree
{
    /// <summary>
    /// 根节点
    /// </summary>
    [SerializeField]
    private RootNode _rootNode;

    /// <summary>
    /// 节点列表
    /// </summary>
    [SerializeField]
    private List<Node> _nodes = new List<Node>();

    public INode RootNode => _rootNode;

    public Type NodeParentType => typeof(Node);

    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="runner"></param>
    /// <returns></returns>
    public Node.NodeStatus Tick(BehaviorTreeRunner runner)
    {
        if(_rootNode.Status == Node.NodeStatus.Running)
        {
            _rootNode.Tick(runner);
        }
        return _rootNode.Status;
    }

    /// <summary>
    /// 添加节点
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public Node AddNode(Type type)
    {
        var node = CreateInstance(type) as Node;
        node.name = type.Name;
        node.Guid = GUID.Generate().ToString();
        _nodes.Add(node);

        AssetDatabase.AddObjectToAsset(node, this);
        AssetDatabase.SaveAssets();
        return node;
    }

    /// <summary>
    /// 移除节点
    /// </summary>
    /// <param name="node"></param>
    public void RemoveNode(Node node)
    {
        _nodes.Remove(node);

        //移除与父节点的连接
        Node parent = FindParent(node, _rootNode);
        DisconnectNode(parent, node);

        AssetDatabase.RemoveObjectFromAsset(node);
        AssetDatabase.SaveAssets();
    }

    /// <summary>
    /// 寻找指定节点的父节点
    /// </summary>
    /// <param name="node"></param>
    /// <returns></returns>
    public Node FindParent(Node target, Node start)
    {
        if(target == start) { return start; }

        var composite = start as CompositeNode;
        if (composite)
        {
            foreach(var child in composite.Childrens)
            {
                var parent = FindParent(target, child);
                if(parent != null) { return parent; }
            };
        }
        else
        {
            var decorator = start as DecoratorNode;
            if (decorator)
            {
                var parent = FindParent(target, decorator.Child);
                if (parent != null) { return parent; }
            }
        }

        return null;
    }

    /// <summary>
    /// 克隆
    /// </summary>
    /// <returns></returns>
    public BehaviorTree Clone()
    {
        var tree = Instantiate(this);
        tree._rootNode = _rootNode.Clone() as RootNode;
        return tree;
    }

    public INode[] GetNodes()
    {
        return _nodes.ToArray();
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
            if(node != null)
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

        Debug.LogError("非法将叶子节点当作父节点");
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

        Debug.LogError("非法将叶子节点当作父节点");
    }

    public void SetRoot()
    {
        _rootNode = AddNode(typeof(RootNode)) as RootNode;
    }
}
