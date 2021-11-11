using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

public class NodeView : UnityEditor.Experimental.GraphView.Node
{
    public Action<NodeView> OnNodeSelected;

    public Action<NodeView> OnDeleted;

    /// <summary>
    /// �ڵ�ʵ��
    /// </summary>
    public INodeModel Node;

    /// <summary>
    /// ����˿�
    /// </summary>
    public Port Input;

    /// <summary>
    /// ����˿�
    /// </summary>
    public Port Output;

    public NodeView(INodeModel node)
    {
        Node = node;
        this.title = node.Name;
        viewDataKey = node.Guid;

        style.left = node.ViewPosition.x;
        style.top = node.ViewPosition.y;

        CreateInputPorts();
        CreateOutputPorts();
    }

    /// <summary>
    /// ��������˿�
    /// </summary>
    private void CreateOutputPorts()
    {
        Output = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(bool));
        Output.portName = "";
        outputContainer.Add(Output);
    }

    /// <summary>
    /// ��������˿�
    /// </summary>
    private void CreateInputPorts()
    {
        Input = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(bool));
        Input.name = "";
        inputContainer.Add(Input);
    }

    public override void SetPosition(Rect newPos)
    {
        base.SetPosition(newPos);
        Node.ViewPosition = new Vector2(newPos.xMin, newPos.yMin);
    }

    public override void OnSelected()
    {
        base.OnSelected();
        OnNodeSelected?.Invoke(this);
    }

    public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
    {
        base.BuildContextualMenu(evt);
        evt.menu.AppendAction("ɾ��", (a) => OnDeleted?.Invoke(this));
    }
}
