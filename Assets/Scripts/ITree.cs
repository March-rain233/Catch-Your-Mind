using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 可树化接口
/// </summary>
public interface ITree
{
    /// <summary>
    /// 根节点
    /// </summary>
    INode RootNode { get; }

    /// <summary>
    /// 节点的根类型
    /// </summary>
    System.Type NodeParentType { get; }

    /// <summary>
    /// 根节点类型
    /// </summary>
    System.Type RootType { get; }

    /// <summary>
    /// 设置根节点
    /// </summary>
    void SetRoot();

    /// <summary>
    /// 获取当前树中的所有节点
    /// </summary>
    /// <returns></returns>
    INode[] GetNodes();

    /// <summary>
    /// 创建指定类型的节点
    /// </summary>
    /// <returns></returns>
    INode CreateNode(System.Type type);

    /// <summary>
    /// 从节点列表中移除指定节点
    /// </summary>
    /// <param name="node"></param>
    void RemoveNode(INode node);

    /// <summary>
    /// 连接节点
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="child"></param>
    void ConnectNode(INode parent, INode child);

    /// <summary>
    /// 断连节点
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="child"></param>
    void DisconnectNode(INode parent, INode child);
}
