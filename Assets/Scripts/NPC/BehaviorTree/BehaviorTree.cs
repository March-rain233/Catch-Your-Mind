using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// ��Ϊ��
/// </summary>
public class BehaviorTree : ScriptableObject
{
    /// <summary>
    /// ���ڵ�
    /// </summary>
    public RootNode RootNode;

    /// <summary>
    /// �ڵ��б�
    /// </summary>
    public List<Node> Nodes = new List<Node>();

    /// <summary>
    /// ����
    /// </summary>
    /// <param name="runner"></param>
    /// <returns></returns>
    public Node.NodeStatus Tick(BehaviorTreeRunner runner)
    {
        if(RootNode.Status == Node.NodeStatus.Running)
        {
            RootNode.Tick(runner);
        }
        return RootNode.Status;
    }

    /// <summary>
    /// ��ӽڵ�
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public Node AddNode(System.Type type)
    {
        Node node = CreateInstance(type) as Node;
        node.name = type.Name;
        node.Guid = GUID.Generate().ToString();
        Nodes.Add(node);

        AssetDatabase.AddObjectToAsset(node, this);
        AssetDatabase.SaveAssets();
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
        Node parent = FindParent(node, RootNode);
        var composite = parent as CompositeNode;
        if (composite)
        {
            composite.Childrens.Remove(node);
        }
        else
        {
            var decorator = parent as DecoratorNode;
            if (decorator)
            {
                decorator.Child = null;
            }
        }

        AssetDatabase.RemoveObjectFromAsset(node);
        AssetDatabase.SaveAssets();
    }

    /// <summary>
    /// Ѱ��ָ���ڵ�ĸ��ڵ�
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
    /// ��¡
    /// </summary>
    /// <returns></returns>
    public BehaviorTree Clone()
    {
        var tree = Instantiate(this);
        tree.RootNode = RootNode.Clone() as RootNode;
        return tree;
    }
}
