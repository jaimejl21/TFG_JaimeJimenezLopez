using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Node : ScriptableObject
{
    public enum Status
    {
        Running,
        Failure,
        Success
    }

    [HideInInspector] public Status status = Status.Running;
    [HideInInspector] public bool started = false;
    [HideInInspector] public string guide;
    [HideInInspector] public Vector2 position;
    [TextArea] public string description;

    public Status Update()
    {
        if (!started)
        {
            OnStart();
            started = true;
        }

        status = OnUpdate();

        if (Status.Failure.Equals(status) || Status.Success.Equals((status)))
        {
            OnStop();
            started = false;
        }

        return status;
    }

    public virtual Node Clone()
    {
        return Instantiate(this);
    }

    protected abstract void OnStart();
    protected abstract Status OnUpdate();
    protected abstract void OnStop();
}
