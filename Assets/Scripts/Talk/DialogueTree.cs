//using NPC;
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEditor.Experimental.GraphView;
//using UnityEngine;

//namespace Dialogue
//{
//    /// <summary>
//    /// ¶Ô»°Ê÷
//    /// </summary>
//    public class DialogueTree : ActionNode, ITree
//    {
//        public INode RootNode => throw new NotImplementedException();

//        public Type NodeParentType => throw new NotImplementedException();

//        public override Port.Capacity Output => throw new NotImplementedException();

//        public Type RootType => throw new NotImplementedException();

//        public override NPC.Node Clone()
//        {
//            var tree = Instantiate(this);
//        }

//        public void ConnectNode(INode parent, INode child)
//        {
//            throw new NotImplementedException();
//        }

//        public INode CreateNode(Type type)
//        {
//            throw new NotImplementedException();
//        }

//        public void DisconnectNode(INode parent, INode child)
//        {
//            throw new NotImplementedException();
//        }

//        public INode[] GetNodes()
//        {
//            throw new NotImplementedException();
//        }

//        public void RemoveNode(INode node)
//        {
//            throw new NotImplementedException();
//        }

//        public void SetRoot()
//        {
//            throw new NotImplementedException();
//        }

//        protected override void OnAbort(BehaviorTreeRunner runner)
//        {
//            throw new NotImplementedException();
//        }

//        protected override void OnEnter(BehaviorTreeRunner runner)
//        {
//            throw new NotImplementedException();
//        }

//        protected override void OnExit(BehaviorTreeRunner runner)
//        {
//            throw new NotImplementedException();
//        }

//        protected override void OnResume(BehaviorTreeRunner runner)
//        {
//            throw new NotImplementedException();
//        }

//        protected override NodeStatus OnUpdate(BehaviorTreeRunner runner)
//        {
//            throw new NotImplementedException();
//        }
//    }
//}
