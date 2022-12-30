using System;
using UnityEditor;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

public class NodeView : UnityEditor.Experimental.GraphView.Node
{
    public Action<NodeView> OnNodeSelected;
    public Node node;
    public Port input;
    public Port output;
    
    public NodeView(Node node) : base("Assets/Scripts/BT-GUI/GUI/BT-Editor/NodeView.uxml")
    {
        this.node = node;
        this.title = node.name;
        this.viewDataKey = node.guide;
        
        style.left = node.position.x;
        style.top = node.position.y;

        CreateInput();
        CreateOutput();

        Label description = this.Q<Label>("description");
        description.bindingPath = "description";
        description.Bind(new SerializedObject(node));
    }

    private void CreateInput()
    {
        if (node is NodeAction)
        {
            input = InstantiatePort(Orientation.Vertical, Direction.Input, Port.Capacity.Single, typeof(bool));
        }
        else if (node is NodeComposite)
        {
            input = InstantiatePort(Orientation.Vertical, Direction.Input, Port.Capacity.Single, typeof(bool));
        }
        else if (node is NodeDecorator)
        {
            input = InstantiatePort(Orientation.Vertical, Direction.Input, Port.Capacity.Single, typeof(bool));
        }
        else if (node is NodeRoot)
        {
            //No input because root node has no input, only childs.
        }

        if (input != null)
        {
            input.portName = "";
            input.style.flexDirection = FlexDirection.Column;
            inputContainer.Add(input);
        }
    }

    private void CreateOutput()
    {
        if (node is NodeAction)
        {
        }
        else if (node is NodeComposite)
        {
            output = InstantiatePort(Orientation.Vertical, Direction.Output, Port.Capacity.Multi, typeof(bool));
        }
        else if (node is NodeDecorator)
        {
            output = InstantiatePort(Orientation.Vertical, Direction.Output, Port.Capacity.Single, typeof(bool));
        }
        else if (node is NodeRoot)
        {
            output = InstantiatePort(Orientation.Vertical, Direction.Output, Port.Capacity.Single, typeof(bool));
        }

        if (output != null)
        {
            output.portName = "";
            output.style.flexDirection = FlexDirection.ColumnReverse;
            outputContainer.Add(output);
        }
    }

    public override void SetPosition(Rect newPosition)
    {
        base.SetPosition(newPosition);
        node.position.x = newPosition.xMin;
        node.position.y = newPosition.yMin;
    }

    public override void OnSelected()
    {
        base.OnSelected();
        if (OnNodeSelected != null)
        {
            OnNodeSelected.Invoke(this);
        }
    }

    public void SortChildren()
    {
        NodeComposite nodeComposite = node as NodeComposite;
        if (nodeComposite)
        {
            nodeComposite.children.Sort(SortByTreePosition);
        }
    }

    private int SortByTreePosition(Node leftNode, Node rightNode)
    {
        return leftNode.position.x < rightNode.position.x ? -1 : 1;
    }

    public void UpdateState()
    {
        RemoveFromClassList("running");
        RemoveFromClassList("failure");
        RemoveFromClassList("success");

        if (Application.isPlaying)
        {
            switch (node.status)
            {
                case Node.Status.Running:
                    if (node.started)
                    {
                        AddToClassList("running");
                    }
                    break;
                case Node.Status.Failure:
                    AddToClassList("failure");
                    break;
                case Node.Status.Success:
                    AddToClassList("success");
                    break;
            }
        }
    }
}
