using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Item;

namespace NPC
{
    /// <summary>
    /// AI基类
    /// </summary>
    /// <remarks>
    /// 执行NPC的行为逻辑，实际上是状态机
    /// </remarks>
    [CreateAssetMenu(fileName = "行为控制器", menuName = "角色/角色行为控制器")]
    public class AI_Controller : SerializedScriptableObject, IProduct
    {
        /// <summary>
        /// 状态字典
        /// </summary>
        [SerializeField]
        private Dictionary<string, BaseState> _states;

        /// <summary>
        /// 状态图
        /// </summary>
        [SerializeField]
        private Dictionary<string, TransitionInfo[]> _graph;

        /// <summary>
        /// 任何状态过渡
        /// </summary>
        [SerializeField]
        private TransitionInfo[] _anyState;

        /// <summary>
        /// 进入初始状态索引
        /// </summary>
        [SerializeField]
        private string _enterState;

        public void OnEnter(BehaviorStateMachine user)
        {
            user.CurState = _enterState;
        }

        public void OnUpdate(BehaviorStateMachine user)
        {
            if(TryGetNextState(user, out string nextState))
            {
                SetState(user, nextState);
            }
            _states[user.CurState].OnUpdate(user);
        }

        /// <summary>
        /// 获取下一个状态
        /// </summary>
        /// <returns>是否改变状态</returns>
        private bool TryGetNextState(BehaviorStateMachine user, out string nextState)
        {
            //先检查任意状态过渡是否符合
            TransitionInfo[] map = _anyState;
            for (int i = 0; i < map.Length; ++i)
            {
                if (map[i].Transition.Reason(user))
                {
                    nextState = map[i].Target;
                    return true;
                }
            }
            //再检查当前状态的过渡是否符合
            map = _graph[user.CurState];
            for(int i = 0; i < map.Length; ++i)
            {
                if (map[i].Transition.Reason(user))
                {
                    nextState = map[i].Target;
                    return true;
                }
            }
            nextState = null;
            return false;
        }

        /// <summary>
        /// 设置为指定状态
        /// </summary>
        /// <param name="user"></param>
        /// <param name="state"></param>
        private void SetState(BehaviorStateMachine user, string state)
        {
            _states[user.CurState].OnExit(user);
            user.CurState = state;
            _states[user.CurState].OnEnter(user);
        }

        /// <summary>
        /// 伤害逻辑
        /// </summary>
        public void Hurt(BehaviorStateMachine user, float hurt, Status status)
        {
            _states[user.CurState].Hurt(user, hurt, status);
        }

        public IProduct Clone()
        {
            return this;
        }

        /// <summary>
        /// 过渡信息
        /// </summary>
        [SerializeField, System.Serializable]
        private struct TransitionInfo
        {
            /// <summary>
            /// 过渡
            /// </summary>
            public BaseTransition Transition;

            /// <summary>
            /// 目标状态
            /// </summary>
            public string Target;
        }
    }
}