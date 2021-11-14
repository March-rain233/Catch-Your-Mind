using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �������ӿ�
/// </summary>
public interface ITree
{
    /// <summary>
    /// ���ڵ�
    /// </summary>
    INode RootNode { get; }

    /// <summary>
    /// �ڵ�ĸ�����
    /// </summary>
    System.Type NodeParentType { get; }

    /// <summary>
    /// ���ڵ�����
    /// </summary>
    System.Type RootType { get; }

    /// <summary>
    /// ���ø��ڵ�
    /// </summary>
    void SetRoot();

    /// <summary>
    /// ��ȡ��ǰ���е����нڵ�
    /// </summary>
    /// <returns></returns>
    INode[] GetNodes();

    /// <summary>
    /// ����ָ�����͵Ľڵ�
    /// </summary>
    /// <returns></returns>
    INode CreateNode(System.Type type);

    /// <summary>
    /// �ӽڵ��б����Ƴ�ָ���ڵ�
    /// </summary>
    /// <param name="node"></param>
    void RemoveNode(INode node);

    /// <summary>
    /// ���ӽڵ�
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="child"></param>
    void ConnectNode(INode parent, INode child);

    /// <summary>
    /// �����ڵ�
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="child"></param>
    void DisconnectNode(INode parent, INode child);
}
