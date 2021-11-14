//using System.Collections;
//using System.Collections.Generic;
//using UnityEditor.Experimental.GraphView;
//using UnityEngine;

//namespace Dialogue
//{
//    public abstract class Node : ScriptableObject, INode
//    {
//        public string Guid { get; set; }

//        public string Name => name;

//        public Vector2 ViewPosition { get; set; }

//        public virtual bool IsRoot => false;

//        public virtual bool IsLeaf => false;

//        public virtual Port.Capacity Input => Port.Capacity.Multi;

//        public virtual Port.Capacity Output=> Port.Capacity.Single;

//        private bool _isStarted = false;

//        /// <summary>
//        /// 进入该节点的条件
//        /// </summary>
//        [SerializeField]
//        private Condition _condition;

//        public abstract INode[] GetChildren();

//        /// <summary>
//        /// 判断进入该节点的条件是否成立
//        /// </summary>
//        /// <param name="tree"></param>
//        /// <returns></returns>
//        protected bool JudgeCondition(DialogueTree tree)
//        {
//            if(_condition == null) { return true; }
//            return _condition.Reason(tree);
//        }

//        public virtual Node Tick(DialogueTree tree)
//        {
//            if(_isStarted == false)
//            {
//                _isStarted = true;
//                OnEnter(tree);
//            }
//            var status = OnUpdate(tree);
//            if (status == NPC.Node.NodeStatus.Running)
//            {
//                return this;
//            }

//            _isStarted = false;
//            OnExit(tree);
//            return SelectChild(tree);
//        }

//        protected virtual void OnEnter(DialogueTree tree) { }

//        protected virtual void OnExit(DialogueTree tree) { }

//        protected abstract NPC.Node.NodeStatus OnUpdate(DialogueTree tree);

//        protected abstract Node SelectChild(DialogueTree tree);

//        public abstract NPC.Node Clone();
//    }
//}
