//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEditor.Experimental.GraphView;
//using UnityEngine;

//namespace TaskTree
//{

//    public class Task : ScriptableObject, INode
//    {
//        public string Guid { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

//        public string Name => throw new NotImplementedException();

//        public Vector2 ViewPosition { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

//        public bool IsRoot => throw new NotImplementedException();

//        public bool IsLeaf => throw new NotImplementedException();

//        public Port.Capacity Input => throw new NotImplementedException();

//        public Port.Capacity Output => throw new NotImplementedException();

//        public event Action<string> OnNameChanged;
//        public event Action<Color> OnStatusChanged;

//        public INode[] GetChildren()
//        {
//            throw new NotImplementedException();
//        }
//    }
//}