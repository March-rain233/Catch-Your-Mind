using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TaskTree
{

    public class TaskTree : ScriptableObject, ITree
    {
        public INode RootNode => throw new NotImplementedException();

        public Type NodeParentType => throw new NotImplementedException();

        public Type RootType => throw new NotImplementedException();

        public void AddNode(INode node)
        {
            throw new NotImplementedException();
        }

        public void ConnectNode(INode parent, INode child)
        {
            throw new NotImplementedException();
        }

        public INode CreateNode(Type type)
        {
            throw new NotImplementedException();
        }

        public void DisconnectNode(INode parent, INode child)
        {
            throw new NotImplementedException();
        }

        public INode[] GetNodes()
        {
            throw new NotImplementedException();
        }

        public void RemoveNode(INode node)
        {
            throw new NotImplementedException();
        }

        public void SetRoot()
        {
            throw new NotImplementedException();
        }

    }
}
