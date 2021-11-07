using System.Collections;
using System.Collections.Generic;
using UnityEngine.UIElements;
using UnityEditor;
using UnityEditor.Experimental.GraphView;

public class BehaviorStateMachineView : GraphView
{
    public new class UxmlFactory : UxmlFactory<BehaviorStateMachineView, UxmlTraits> { }

    public BehaviorStateMachineView()
    {

    }
}
