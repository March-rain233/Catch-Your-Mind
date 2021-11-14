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
    /// 节点实例
    /// </summary>
    public INode Node;

    /// <summary>
    /// 输入端口
    /// </summary>
    public Port Input;

    /// <summary>
    /// 输出端口
    /// </summary>
    public List<Port> Output = new List<Port>();

    public NodeView(INode node)
    {
        Node = node;
        title = node.Name;
        viewDataKey = node.Guid;

        style.left = node.ViewPosition.x;
        style.top = node.ViewPosition.y;

        CreateInputPorts();
        //CreateOutputPorts();
        if (node.Output == Port.Capacity.Single)
        {
            CreateOutputPorts();
        }
        else
        {
            Button btn = new Button(() => { AddOutputPort(); });
            btn.text = "添加输出端口";
            titleButtonContainer.Add(btn);
            var n = node.GetChildren().Length;
            for(int i = 0; i < n; ++i)
            {
                AddOutputPort();
            }
        }

        node.OnStatusChanged += status =>
        {
            Debug.Log(status);
            switch (status)
            {
                case NPC.Node.NodeStatus.Success:
                    style.backgroundColor = Color.green;
                    break;
                case NPC.Node.NodeStatus.Failure:
                    style.backgroundColor = Color.red;
                    break;
                case NPC.Node.NodeStatus.Running:
                    style.backgroundColor = Color.blue;
                    break;
                case NPC.Node.NodeStatus.Aborting:
                    style.backgroundColor = Color.yellow;
                    break;
            }
        };
        node.OnNameChanged += newName => title = newName;
    }

    /// <summary>
    /// 创建输出端口
    /// </summary>
    private void CreateOutputPorts()
    {
        if (Node.IsLeaf) { return; }
        Output.Add(InstantiatePort(Orientation.Horizontal, Direction.Output, Node.Output, null));
        Output[Output.Count - 1].portName = "";
        outputContainer.Add(Output[Output.Count - 1]);
    }

    /// <summary>
    /// 添加输出端口
    /// </summary>
    private void AddOutputPort()
    {
        Output.Add(InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, null));
        Output[Output.Count - 1].portName = outputContainer.childCount.ToString();
        outputContainer.Add(Output[Output.Count - 1]);
    }

    /// <summary>
    /// 创建输入端口
    /// </summary>
    private void CreateInputPorts()
    {
        if (Node.IsRoot) { return; }
        Input = InstantiatePort(Orientation.Horizontal, Direction.Input, Node.Input, null);
        //Debug.Log(title + (Input==null).ToString());
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
        if (!Node.IsRoot)
        {
            evt.menu.AppendAction("删除", (a) => OnDeleted?.Invoke(this));
        }
    }

}
