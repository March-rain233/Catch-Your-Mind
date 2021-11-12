using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��Ͻڵ�
/// </summary>
public abstract class CompositeNode : Node
{

    /// <summary>
    /// �ýڵ��Ϸ�ʽ
    /// </summary>
    public AbortType AbortType => _abortType;
    private AbortType _abortType;

    public List<Node> Childrens = new List<Node>();

    public override UnityEditor.Experimental.GraphView.Port.Capacity Output => UnityEditor.Experimental.GraphView.Port.Capacity.Multi;

    protected override void OnAbort(BehaviorTreeRunner runner)
    {
        Childrens.ForEach(child => { if (child.Status == NodeStatus.Running) child.Abort(runner); });
    }

    public override INode[] GetChildren()
    {
        return Childrens.ToArray();
    }

    public override Node Clone()
    {
        var node = Instantiate(this);
        for (int i = 0; i < Childrens.Count; ++i)
        {
            Childrens[i] = Childrens[i].Clone();
        }
        return node;
    }
}
/// <summary>
/// ��Ϸ�ʽ
/// </summary>
public enum AbortType
{
    None,
    LowerPriority,
    Self,
    Both
}