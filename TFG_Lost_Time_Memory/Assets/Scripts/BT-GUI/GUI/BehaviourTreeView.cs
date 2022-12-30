using System;
using System.Linq;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

public class BehaviourTreeView : GraphView
{
    public Action<NodeView> OnNodeSelected;
    public new class UxmlFactory : UxmlFactory<BehaviourTreeView, UxmlTraits>
    {
    }

    BehaviourTree _behaviourTree;

    public BehaviourTreeView()
    {
        Insert(0, new GridBackground());

        this.AddManipulator(new ContentZoomer());
        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());

        var styleSheet =
            AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Scripts/BT-GUI/GUI/BT-Editor/BehaviourTreeEditor.uss");
        styleSheets.Add(styleSheet);
    }

    internal void PopulateView(BehaviourTree tree)
    {
        this._behaviourTree = tree;

        graphViewChanged -= OnGraphViewChanged;
        DeleteElements(graphElements);
        graphViewChanged += OnGraphViewChanged;

        if (tree.root == null)
        {
            tree.root = tree.CreateNode(typeof(NodeRoot)) as NodeRoot;
            EditorUtility.SetDirty(tree);
            AssetDatabase.SaveAssets();
        }
        
        //Create node view
        tree.nodeList.ForEach(node => CreateNodeView(node));
        
        //Create edges
        tree.nodeList.ForEach(node =>
        {
            var children = tree.GetChildrenNodes(node);
            children.ForEach(child =>
            {
                NodeView nodeParentView = FindNodeView(node);
                NodeView nodeChildView = FindNodeView(child);

                Edge edge = nodeParentView.output.ConnectTo(nodeChildView.input);
                AddElement(edge);
            });
        });
    }

    NodeView FindNodeView(Node node)
    {
        return GetNodeByGuid(node.guide) as NodeView;
    }

    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
    {
        return ports.ToList()
            .Where(endPort => endPort.direction != startPort.direction && endPort.node != startPort.node).ToList();
    }

    private GraphViewChange OnGraphViewChanged(GraphViewChange graphviewchange)
    {
        if (graphviewchange.elementsToRemove != null)
        {
            graphviewchange.elementsToRemove.ForEach(elem =>
            {
                NodeView nodeView = elem as NodeView;
                if (nodeView != null)
                {
                    _behaviourTree.DeleteNode(nodeView.node);
                }

                Edge edge = elem as Edge;
                if (edge != null)
                {
                    NodeView nodeViewParent = edge.output.node as NodeView;
                    NodeView nodeViewChild = edge.input.node as NodeView;
                    _behaviourTree.RemoveChildNode(nodeViewParent.node, nodeViewChild.node);
                }
            });
        }

        if (graphviewchange.edgesToCreate != null)
        {
            graphviewchange.edgesToCreate.ForEach(edge =>
            {
                NodeView nodeViewParent = edge.output.node as NodeView;
                NodeView nodeViewChild = edge.input.node as NodeView;
                _behaviourTree.AddChildNode(nodeViewParent.node, nodeViewChild.node);
            });
        }

        if (graphviewchange.movedElements != null)
        {
            nodes.ForEach((n) =>
            {
                NodeView nodeView = n as NodeView;
                nodeView.SortChildren();
            });
        }
        return graphviewchange;
    }

    public override void BuildContextualMenu(ContextualMenuPopulateEvent contextualMenuPopulateEvent)
    {
        //base.BuildContextualMenu(evt);
        //Creamos una accion de creación de nodo existente por cada nodo
        {
            var types = TypeCache.GetTypesDerivedFrom<NodeAction>();
            foreach (var type in types)
            {
                contextualMenuPopulateEvent.menu.AppendAction($"[{type.BaseType.Name}] {type.Name}",
                    (a) => CreateNode(type));
            }
        }
        {
            var types = TypeCache.GetTypesDerivedFrom<NodeComposite>();
            foreach (var type in types)
            {
                contextualMenuPopulateEvent.menu.AppendAction($"[{type.BaseType.Name}] {type.Name}",
                    (a) => CreateNode(type));
            }
        }
        {
            var types = TypeCache.GetTypesDerivedFrom<NodeDecorator>();
            foreach (var type in types)
            {
                contextualMenuPopulateEvent.menu.AppendAction($"[{type.BaseType.Name}] {type.Name}",
                    (a) => CreateNode(type));
            }
        }
    }

    void CreateNode(Type type)
    {
        Node node = _behaviourTree.CreateNode(type);
        CreateNodeView(node);
    }

    void CreateNodeView(Node node)
    {
        NodeView nodeView = new NodeView(node);
        nodeView.OnNodeSelected = OnNodeSelected;
        AddElement(nodeView);
    }

    public void UpdateNodeState()
    {
        nodes.ForEach(n =>
        {
            NodeView view = n as NodeView;
            view.UpdateState();
        });
    }
}