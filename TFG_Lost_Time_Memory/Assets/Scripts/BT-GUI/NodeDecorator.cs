using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NodeDecorator : Node
{
    [HideInInspector] public Node child;

    public override Node Clone()
    {
        NodeDecorator nodeDecorator = Instantiate(this);
        nodeDecorator.child = child.Clone();
        return nodeDecorator;
    }
}
