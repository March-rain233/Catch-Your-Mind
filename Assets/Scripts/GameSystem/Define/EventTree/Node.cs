using System;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor.Experimental.GraphView;
#endif
using UnityEngine;

namespace EventTree
{

    public abstract class Node : ScriptableObject, INode
    {
        public string Guid { get; set; }

        public string Name => name;

        public Vector2 ViewPosition { get; set; }

        public virtual bool IsRoot => false;

        public virtual bool IsLeaf => false;

#if UNITY_EDITOR
        public virtual Port.Capacity Input => Port.Capacity.Multi;

        public virtual Port.Capacity Output => Port.Capacity.Multi;
#endif

        public event Action<string> OnNameChanged;
        public event Action<Color> OnStatusChanged;

        public List<Node> Nodes = new List<Node>();

        public INode[] GetChildren() { return Nodes.ToArray(); }

        public void Tick(string eventName, EventCenter.EventArgs eventArgs)
        {
            EventHandler(eventName, eventArgs);
            SendChildrens(eventName, eventArgs);
        }

        protected virtual void SendChildrens(string eventName, EventCenter.EventArgs eventArgs)
        {
            if (Nodes == null || Nodes.Count <= 0) { return; }
            Nodes.ForEach(node => node.Tick(eventName, eventArgs));
        }

        protected virtual void EventHandler(string eventName, EventCenter.EventArgs eventArgs) { }
    }
}
