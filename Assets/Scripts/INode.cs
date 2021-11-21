using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public interface INode
{
    /// <summary>
    /// 唯一标识符
    /// </summary>
    string Guid { get; set; }

    /// <summary>
    /// 节点名称
    /// </summary>
    string Name { get; }

    /// <summary>
    /// 节点在视图上的位置
    /// </summary>
    Vector2 ViewPosition { get; set; }

    /// <summary>
    /// 是否为根节点
    /// </summary>
    bool IsRoot { get; }

    /// <summary>
    /// 是否为叶节点
    /// </summary>
    bool IsLeaf { get; }

    /// <summary>
    /// 当前节点的输入数量
    /// </summary>
    Port.Capacity Input { get; }

    /// <summary>
    /// 当前节点的输出数量
    /// </summary>
    Port.Capacity Output { get; }

    public event System.Action<string> OnNameChanged;

    public event System.Action<NodeStatus> OnStatusChanged;

    /// <summary>
    /// 获取子节点
    /// </summary>
    /// <returns></returns>
    INode[] GetChildren();
}

/// <summary>
/// 节点状态
/// </summary>
[System.Serializable]
public enum NodeStatus
{
    Success,
    Failure,
    Running,
    Aborting,
}
