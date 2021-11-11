using Item;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPC
{
    /// <summary>
    /// 角色状态基类
    /// </summary>
    /// <remarks>
    /// 请将所有字段设为外界可读以便过渡类可以监视
    /// </remarks>
    public abstract class BaseState : ScriptableObject, INodeModel
    {
        /// <summary>
        /// 视图的位置
        /// </summary>
        public Vector2 ViewPosition { get; set; }

        /// <summary>
        /// 标识
        /// </summary>
        public string Guid { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get => name; }

        /// <summary>
        /// 过渡
        /// </summary>
        public List<Transition> Transitions = new List<Transition>();

        /// <summary>
        /// 获取下一个状态
        /// </summary>
        /// <returns>是否改变状态</returns>
        public bool TryGetNextState(BehaviorStateMachine user, out BaseState nextState)
        {
            foreach(var transition in Transitions)
            {
                if (transition.Reason(user))
                {
                    nextState = transition.EndState;
                    return true;
                }
            }
            nextState = null;
            return false;
        }

        /// <summary>
        /// 逻辑执行函数
        /// </summary>
        /// <returns>下一状态</returns>
        public abstract void OnUpdate(BehaviorStateMachine user);

        /// <summary>
        /// 当进入状态时
        /// </summary>
        public virtual void OnEnter(BehaviorStateMachine user)
        {
            Debug.Log($"进入{this.name}");
        }

        /// <summary>
        /// 当脱离状态时
        /// </summary>
        public virtual void OnExit(BehaviorStateMachine user)
        {
            Debug.Log($"退出{this.name}");
        }

        /// <summary>
        /// 当遭到伤害时的计算逻辑
        /// </summary>
        /// <param name="user">承受者</param>
        /// <param name="hurt">攻击值</param>
        /// <param name="status">附加状态</param>
        public virtual void Hurt(BehaviorStateMachine user, float hurt, Status status)
        {
        }
    }
}