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
            for(int i = 0; i < children.Length; ++i)
            {
                if (children[i] == null) { return; }
                NodeView childView = FindNodeView(children[i]);
                //Debug.Log($"{children[i].Name} {childView.Node.Name}");

                Edge edge = parentView.Output[i].ConnectTo(childView.Input);
                AddElement(edge);
            }
        });
    }

    /// <summary>
    /// 删除指定节点
    /// </summary>
    /// <param name="node"></param>
    private void DeleteNodeView(NodeView node)
    {
        RemoveElement(node);
        var es = edges.ToList();
        for(int i = es.Count-1; i >= 0 ; --i)
        {
            if(es[i].input.node == node || es[i].output.node == node)
            {
                RemoveElement(es[i]);
            }
        }
        _tree.RemoveNode(node.Node);
    }

    /// <summary>
    /// 根据传入节点创建节点视图
    /// </summary>
    /// <param name="node"></param>
    private NodeView CreateNodeView(INode node)
    {
        NodeView nodeView = new NodeView(node);
        nodeView.OnNodeSelected = OnElementSelected;
        nodeView.OnDeleted = DeleteNodeView;
        AddElement(nodeView);
        return nodeView;
    }

    /// <summary>
    /// 创建节点
    /// </summary>
    /// <param name="type"></param>
    private NodeView CreateNode(Type type)
    {
        INode node = _tree.CreateNode(type);
        return CreateNodeView(node);
    }

    /// <summary>
    /// 添加子树
    /// </summary>
    /// <param name="subnode"></param>
    public void AddSubtree(INode subnode)
    {
        List<INode> nodes = new List<INode>();
        Stack<INode> stack = new Stack<INode>();
        stack.Push(subnode);
        while (stack.Count > 0)
        {
            var node = stack.Pop();
            Array.ForEach(node.GetChildren(), child => { if (!nodes.Contains(child)) { stack.Push(child); } });
            nodes.Add(node);
        }


        nodes.ForEach(n =>
        {
            _tree.AddNode(n);
            CreateNodeView(n);
        });

        nodes.ForEach(n =>
        {
            var children = n.GetChildren();
            if (children == null || children.Length == 0) { return; }
            NodeView parentView = FindNodeView(n);
            for (int i = 0; i < children.Length; ++i)
            {
                if (children[i] == null) { return; }
                NodeView childView = FindNodeView(children[i]);
                //Debug.Log($"{children[i].Name} {childView.Node.Name}");

                Edge edge = parentView.Output[i].ConnectTo(childView.Input);
                AddElement(edge);
            }
        });
    }

    /// <summary>
    /// 整理图表
    /// </summary>
    public void Sort_1()
    {
        Vector2 ori = Vector2.zero;
        float yDifference = 150;
        float xDifference = 250;
        int lenth = 1;
        int ylenth = 0;
        
        List<INode> sorted = new List<INode>();

        Queue<INode> queue = new Queue<INode>();
        Queue<INode> next = new Queue<INode>();
        queue.Enqueue(_tree.RootNode);
        while (queue.Count > 0)
        {
            var node = queue.Dequeue();
            Array.ForEach(node.GetChildren(), child => { if (!sorted.Contains(child)) { next.Enqueue(child); } });
            sorted.Add(node);
            FindNodeView(node).SetPosition(new Rect(ori.x + xDifference * lenth, ori.y + yDifference * ylenth++, 0, 0));
            if(queue.Count <= 0)
            {
                queue = next;
                next = new Queue<INode>();
                ylenth = 0;
                ++lenth;
            }
        }
    }

    public void Sort_2()
    {
        Vector2 ori = Vector2.zero;
        float yDifference = 150;
        float xDifference = 250;
        int lenth = 1;
        int ylenth = 0;

        List<INode> sorted = new List<INode>();

        Queue<INode> queue = new Queue<INode>();
        Queue<INode> next = new Queue<INode>();
        queue.Enqueue(_tree.RootNode);
        while (queue.Count > 0)
        {
            var node = queue.Dequeue();
            Array.ForEach(node.GetChildren(), child => { if (!sorted.Contains(child)) { next.Enqueue(child); } });
            sorted.Add(node);
            FindNodeView(node).SetPosition(new Rect(ori.x + xDifference * lenth, ori.y + yDifference * ylenth++, 0, 0));
            if (queue.Count <= 0)
            {
                queue = next;
                next = new Queue<INode>();
                ylenth = -(queue.Count / 2);
                ++lenth;
            }
        }
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
            //if (type == _tree.RootType) { continue; }
            Type parent = type;
            StringBuilder sb = new StringBuilder();
            while (parent != _tree.NodeParentType)
            {
                sb.Insert(0, '/' + parent.Name);
                parent = parent.BaseType;
            }
            sb.Remove(0, 1);
            if(type == _tree.RootType) { sb.Append("（为了防止特殊情况而保留。看清楚了，这是根节点，不要乱加）"); }
            evt.menu.AppendAction(sb.ToString(), (a) =>
            {
                CreateNode(type);
            });
        }
    }
}
