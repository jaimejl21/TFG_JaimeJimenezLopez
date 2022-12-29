using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviourTreeRunner : MonoBehaviour
{
    public BehaviourTree _behaviourTree;
    // Start is called before the first frame update
    void Start()
    {
        _behaviourTree = _behaviourTree.Clone();
    }

    // Update is called once per frame
    void Update()
    {
        _behaviourTree.Update();
    }
}
