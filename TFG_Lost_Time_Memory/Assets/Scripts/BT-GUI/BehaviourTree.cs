using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu()]
public class BehaviourTree : ScriptableObject
{
    public Node root;
    public Node.Status statusBehaviorTree = Node.Status.Running;
    public List<Node> nodeList = new List<Node>();

    public Node.Status Update()
    {
        if (Node.Status.Running.Equals(root.status))
        {
            statusBehaviorTree = root.Update();
        }

        return statusBehaviorTree;
    }

    public Node CreateNode(Type type)
    {
        Node node = CreateInstance(type) as Node;
        node.name = type.Name;
        node.guide = GUID.Generate().ToString();
        nodeList.Add(node);

        AssetDatabase.AddObjectToAsset(node, this);
        AssetDatabase.SaveAssets();

        return node;
    }

    public void DeleteNode(Node node)
    {
        nodeList.Remove(node);
        AssetDatabase.RemoveObjectFromAsset(node);
        AssetDatabase.SaveAssets();
    }

    public void AddChildNode(Node parent, Node child)
    {
        NodeDecorator nodeDecorator = parent as NodeDecorator;
        if (nodeDecorator)
        {
            nodeDecorator.child = child;
        }
        
        NodeRoot nodeRoot = parent as NodeRoot;
        if (nodeRoot)
        {
            nodeRoot.child = child;
        }

        NodeComposite nodeComposite = parent as NodeComposite;
        if (nodeComposite)
        {
            nodeComposite.children.Add(child);
        }
    }

    public void RemoveChildNode(Node parent, Node child)
    {
        NodeDecorator nodeDecorator = parent as NodeDecorator;
        if (nodeDecorator)
        {
            nodeDecorator.child = null;
        }
        
        NodeRoot nodeRoot = parent as NodeRoot;
        if (nodeRoot)
        {
            nodeRoot.child = null;
        }


        NodeComposite nodeComposite = parent as NodeComposite;
        if (nodeComposite)
        {
            nodeComposite.children.Remove(child);
        }
    }

    public List<Node> GetChildrenNodes(Node parent)
    {
        List<Node> children = new List<Node>();

        NodeDecorator nodeDecorator = parent as NodeDecorator;
        if (nodeDecorator && nodeDecorator.child != null)
        {
            children.Add(nodeDecorator.child);
        }

        NodeRoot nodeRoot = parent as NodeRoot;
        if (nodeRoot && nodeRoot.child != null)
        {
            children.Add(nodeRoot.child);
        }

        NodeComposite nodeComposite = parent as NodeComposite;
        if (nodeComposite)
        {
            return nodeComposite.children;
        }

        return children;
    }

    public BehaviourTree Clone()
    {
        BehaviourTree tree = Instantiate(this);
        tree.root = tree.root.Clone();
        return tree;
    }
}
