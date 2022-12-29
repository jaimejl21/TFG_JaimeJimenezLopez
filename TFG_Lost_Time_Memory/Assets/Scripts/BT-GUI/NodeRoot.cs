using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeRoot : Node
{
    public Node child;
    protected override void OnStart()
    {
    }

    protected override Status OnUpdate()
    {
        return child.Update();
    }

    protected override void OnStop()
    {
    }

    public override Node Clone()
    {
        NodeRoot nodeRoot = Instantiate(this);
        nodeRoot.child = child.Clone();
        return nodeRoot;
    }
}
