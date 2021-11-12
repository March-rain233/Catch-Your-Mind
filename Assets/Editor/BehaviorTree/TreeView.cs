using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using System;
using System.Linq;
using System.Reflection;
using System.Text;

public class TreeView : GraphView
{
    public new class UxmlFactory : UxmlFactory<TreeView, UxmlTraits> { }

    internal Action<GraphElement> OnElementSelected;

    private ITree _tree;

    public TreeView()
    {
        Insert(0, new GridBackground());

        this.AddManipulator(new ContentZoomer());
        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());

        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Editor/BehaviorTree/BehaviorTreeEditor.uss");
        styleSheets.Add(styleSheet);
    }

    /// <summary>
    /// 获取节点视图
    /// </summary>
    /// <param name="node"></param>
    /// <returns></returns>
    NodeView FindNodeView(INode node)
    {
        return GetNodeByGuid(node.Guid) as NodeView;
    }

    /// <summary>
    /// 当选择的tree改变时，重新加载视图
    /// </summary>
    /// <param name="tree"></param>
    internal void PopulateView(ITree tree)
    {
        _tree = tree;

        graphViewChanged -= OnGraphViewChanged;
        DeleteElements(graphElements.ToList());
        graphViewChanged += OnGraphViewChanged;

        if(tree.RootNode == null)
        {
            tree.SetRoot();
        }

        Array.ForEach(_tree.GetNodes(), n => CreateNodeView(n));

        Array.ForEach(_tree.GetNodes(),n =>
        {
            var children = n.GetChildren();
            if(children == null || children.Length == 0) { return; }
            NodeView parentView = FindNodeView(n);
            Array.ForEach(children, child =>
            {
                NodeView childView = FindNodeView(child);

                Edge edge = parentView.Output.ConnectTo(childView.Input);
                AddElement(edge);
            });
        });
    }

    /// <summary>
    /// 删除指定节点
    /// </summary>
    /// <param name="node"></param>
    private void DeleteNodeView(NodeView node)
    {
        RemoveElement(node);
        _tree.RemoveNode(node.Node);
    }

    /// <summary>
    /// 根据传入节点创建节点视图
    /// </summary>
    /// <param name="node"></param>
    private void CreateNodeView(INode node)
    {
        NodeView nodeView = new NodeView(node);
        nodeView.OnNodeSelected = OnElementSelected;
        nodeView.OnDeleted = DeleteNodeView;
        AddElement(nodeView);
    }

    /// <summary>
    /// 创建节点
    /// </summary>
    /// <param name="type"></param>
    private void CreateNode(Type type)
    {
        INode node = _tree.CreateNode(type);
        CreateNodeView(node);
    }

    /// <summary>
    /// 当图发生变化时
    /// </summary>
    /// <param name="graphViewChange"></param>
    /// <returns></returns>
    private GraphViewChange OnGraphViewChanged(GraphViewChange graphViewChange)
    {
        //当元素被移除
        if (graphViewChange.elementsToRemove != null)
        {
            graphViewChange.elementsToRemove.ForEach(elem => {
                //移除点
                NodeView nodeView = elem as NodeView;
                if (nodeView != null)
                {
                    _tree.RemoveNode(nodeView.Node);
                }

                //移除边
                Edge edge = elem as Edge;
                if (edge != null)
                {
                    NodeView parentView = edge.output.node as NodeView;
                    NodeView childView = edge.input.node as NodeView;
                    _tree.DisconnectNode(parentView.Node, childView.Node);
                }
            });
        }

        //当边被创建
        if (graphViewChange.edgesToCreate != null)
        {
            graphViewChange.edgesToCreate.ForEach(edge =>
            {
                NodeView parentView = edge.output.node as NodeView;
                NodeView childView = edge.input.node as NodeView;
                _tree.ConnectNode(parentView.Node, childView.Node);
            });
        }

        return graphViewChange;
    }

    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
    {
        var list = ports.ToList().Where(endPort =>
        endPort.direction != startPort.direction
        && endPort.node != startPort.node).ToList();
        return list;
    }

    public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
    {
        //base.BuildContextualMenu(evt);
        if(_tree == null) { return; }

        var types = TypeCache.GetTypesDerivedFrom(_tree.NodeParentType);
        foreach (var type in types)
        {
            if (type.IsAbstract) { continue; }
            Type parent = type;
            StringBuilder sb = new StringBuilder();
            while(parent != _tree.NodeParentType)
            {
                sb.Insert(0, '/' + parent.Name);
                parent = parent.BaseType;
            }
            sb.Remove(0, 1);
            evt.menu.AppendAction(sb.ToString(), (a) => CreateNode(type));
        }
    }
}
