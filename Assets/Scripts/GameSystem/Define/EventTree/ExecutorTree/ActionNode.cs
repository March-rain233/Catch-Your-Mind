using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using Sirenix.OdinInspector;

namespace EventTree
{

    public abstract class ActionNode : SerializedScriptableObject, INode
    {
        public string Guid { get; set; }

        public string Name => name;

        public Vector2 ViewPosition { get; set; }

        public virtual bool IsRoot => false;

        public virtual bool IsLeaf => false;

        public virtual Port.Capacity Input => Port.Capacity.Single;

        public virtual Port.Capacity Output => Port.Capacity.Multi;

        public event Action<string> OnNameChanged;
        public event Action<Color> OnStatusChanged;

        public List<ActionNode> Nodes = new List<ActionNode>();

        public INode[] GetChildren() { return Nodes.ToArray(); }

        public void Tick(string eventName, EventCenter.EventArgs eventArgs)
        {
            EventHandler(eventName, eventArgs);
            SendChildrens(eventName, eventArgs);
        }

        private void SendChildrens(string eventName, EventCenter.EventArgs eventArgs)
        {
            if (Nodes == null || Nodes.Count <= 0) { return; }
            Nodes.ForEach(node => node.Tick(eventName, eventArgs));
        }

        protected abstract void EventHandler(string eventName, EventCenter.EventArgs eventArgs);
    }
}
