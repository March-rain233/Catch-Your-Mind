using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public interface INode
{
    /// <summary>
    /// Ψһ��ʶ��
    /// </summary>
    string Guid { get; set; }

    /// <summary>
    /// �ڵ�����
    /// </summary>
    string Name { get; }

    /// <summary>
    /// �ڵ�����ͼ�ϵ�λ��
    /// </summary>
    Vector2 ViewPosition { get; set; }

    /// <summary>
    /// �Ƿ�Ϊ���ڵ�
    /// </summary>
    bool IsRoot { get; }

    /// <summary>
    /// �Ƿ�ΪҶ�ڵ�
    /// </summary>
    bool IsLeaf { get; }

    /// <summary>
    /// ��ǰ�ڵ����������
    /// </summary>
    Port.Capacity Input { get; }

    /// <summary>
    /// ��ǰ�ڵ���������
    /// </summary>
    Port.Capacity Output { get; }

    public event System.Action<string> OnNameChanged;

    public event System.Action<NodeStatus> OnStatusChanged;

    /// <summary>
    /// ��ȡ�ӽڵ�
    /// </summary>
    /// <returns></returns>
    INode[] GetChildren();
}

/// <summary>
/// �ڵ�״̬
/// </summary>
[System.Serializable]
public enum NodeStatus
{
    Success,
    Failure,
    Running,
    Aborting,
}
