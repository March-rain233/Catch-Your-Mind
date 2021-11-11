using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviorTreeRunner : MonoBehaviour
{
    public BehaviorTree BehaviorTree;

    private void Awake()
    {
        BehaviorTree = BehaviorTree.Clone();
    }

    private void Update()
    {
        BehaviorTree.Tick(this);
    }
}
