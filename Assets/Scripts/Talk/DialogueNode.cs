using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Dialogue
{
    public class DialogueNode : ScriptableObject, INode
    {
        public string Guid { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        public string Name => throw new System.NotImplementedException();

        public Vector2 ViewPosition { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        public bool IsRoot => throw new System.NotImplementedException();

        public bool IsLeaf => throw new System.NotImplementedException();

        public Port.Capacity Input => throw new System.NotImplementedException();

        public Port.Capacity Output => throw new System.NotImplementedException();

        public INode[] GetChildren()
        {
            throw new System.NotImplementedException();
        }
    }
}
