using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class NodeDebugger : NodeAction
{
    public string log;
    protected override void OnStart()
    {
       Debug.Log($"{log}");
    }

    protected override Status OnUpdate()
    {
        Debug.Log($"{log}");
        return Status.Success;
    }

    protected override void OnStop()
    {
        Debug.Log($"{log}");
    }
}
