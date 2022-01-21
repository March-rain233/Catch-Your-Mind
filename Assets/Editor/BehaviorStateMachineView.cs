#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UIElements;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using System.Linq;
using NPC;
using System;
using UnityEngine;

public class BehaviorStateMachineView : GraphView
{
    public new class UxmlFactory : UxmlFactory<BehaviorStateMachineView, UxmlTraits> { }

    public Action<GraphElement> OnNodeSelected;

    private StateMachineController _controller;

    public BehaviorStateMachineView()
    {
        Insert(0, new GridBackground());

        this.AddManipulator(new ContentZoomer());
        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());

        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Editor/BehaviorStateMachineEditor.uss");
        styleSheets.Add(styleSheet);
    }

    NodeView FindNodeView(BaseState state)
    {
        return GetNodeByGuid(state.Guid) as NodeView;
    }

    internal void PopulateView(StateMachineController controller)
    {
        _controller = controller;

        graphViewChanged -= OnGraphViewChanged;
        DeleteElements(graphElements.ToList());
        graphViewChanged += OnGraphViewChanged;

        UnityEditor.Experimental.GraphView.Node root = new UnityEditor.Experimental.GraphView.Node();
        root.name = root.title = "Entry";

        root.outputContainer.Add(root.InstantiatePort(Orientation.Horizontal,
            UnityEditor.Experimental.GraphView.Direction.Output, Port.Capacity.Single, typeof(bool)));
        AddElement(root);

        _controller.States.ForEach(n => CreateNodeView(n));

        _controller.States.ForEach(n =>
        {
            var transitions = n.Transitions;
            NodeView start = FindNodeView(n);
            int i = 0;
            transitions.ForEach(t =>
            {
                NodeView end = FindNodeView(t.EndState);

                Edge edge = start.Output[i++].ConnectTo(end.Input);
                AddElement(edge);
            });
        });
    }

    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
    {
        var list = ports.ToList().Where(endPort =>
        endPort.direction != startPort.direction
        && endPort.node != startPort.node).ToList();
        return list;
    }

    private GraphViewChange OnGraphViewChanged(GraphViewChange graphViewChange)
    {
        if (graphViewChange.elementsToRemove != null)
        {
            graphViewChange.elementsToRemove.ForEach(elem =>
            {
                NodeView nodeView = elem as NodeView;
                if (nodeView != null)
                {
                    _controller.DeleteState(nodeView.Node as BaseState);
                }

                Edge edge = elem as Edge;
                if (edge != null)
                {
                    NodeView parentView = edge.output.node as NodeView;
                    if (parentView == null)
                    {
                        _controller.EnterState = 0;
                        return;
                    }
                    NodeView childView = edge.input.node as NodeView;
                    _controller.RemoveTransition(parentView.Node as BaseState, childView.Node as BaseState);
                }
            });
        }

        if (graphViewChange.edgesToCreate != null)
        {
            graphViewChange.edgesToCreate.ForEach(edge =>
            {
                NodeView parentView = edge.output.node as NodeView;
                NodeView childView = edge.input.node as NodeView;
                if (parentView == null)
                {
                    _controller.EnterState = _controller.States.IndexOf(childView.Node as BaseState);
                    return;
                }
                _controller.AddTransition(parentView.Node as BaseState, childView.Node as BaseState);
            });
        }

        return graphViewChange;
    }

    public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
    {
        //base.BuildContextualMenu(evt);
        var types = TypeCache.GetTypesDerivedFrom<BaseState>();
        foreach (var type in types)
        {
            evt.menu.AppendAction($"[{type.BaseType.Name}]{type.Name}", (a) => CreateNode(type));
        }
    }

    private void CreateNode(Type type)
    {
        BaseState state = _controller.CreateState(type);
        CreateNodeView(state);
    }

    private void CreateNodeView(BaseState node)
    {
        NodeView nodeView = new NodeView(node);
        nodeView.OnNodeSelected = OnNodeSelected;
        nodeView.OnDeleted = DeleteNode;
        AddElement(nodeView);
    }

    private void DeleteNode(NodeView nodeView)
    {
        var delete = edges.ToList().Where(e => e.input.node == nodeView || e.output.node == nodeView);
        DeleteElements(delete);
        RemoveElement(nodeView);
        _controller.DeleteState(nodeView.Node as BaseState);
    }
}
#endif