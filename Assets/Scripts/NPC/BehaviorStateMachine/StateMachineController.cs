using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Item;
using UnityEditor;

namespace NPC
{
    /// <summary>
    /// AI基类
    /// </summary>
    /// <remarks>
    /// 执行NPC的行为逻辑，实际上是状态机
    /// </remarks>
    [CreateAssetMenu(fileName = "行为控制器", menuName = "角色/角色行为控制器")]
    public class StateMachineController : SerializedScriptableObject, IProduct
    {
        /// <summary>
        /// 状态列表
        /// </summary>
        [SerializeField]
        public List<BaseState> States;


        /// <summary>
        /// 任何状态过渡
        /// </summary>
        [SerializeField]
        public List<Transition> AnyState;

        /// <summary>
        /// 进入初始状态
        /// </summary>
        [SerializeField]
        public int EnterState;

        /// <summary>
        /// 删除状态
        /// </summary>
        /// <param name="state"></param>
        public void DeleteState(BaseState state)
        {
            States.Remove(state);

            state.Transitions.RemoveAll(t => 
            { 
                AssetDatabase.RemoveObjectFromAsset(t);
                return true; 
            });
            States.ForEach(s => s.Transitions.RemoveAll(t =>
            {
                if(t.EndState == state)
                {
                    AssetDatabase.RemoveObjectFromAsset(t);
                    return true;
                }
                return false;
            }));

            AssetDatabase.RemoveObjectFromAsset(state);
            AssetDatabase.SaveAssets();
        }

        /// <summary>
        /// 创建状态
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public BaseState CreateState(System.Type type)
        {
            var state = CreateInstance(type) as BaseState;
            state.name = type.Name;
            state.Guid = GUID.Generate().ToString();
            States.Add(state);

            AssetDatabase.AddObjectToAsset(state, this);
            AssetDatabase.SaveAssets();
            return state;
        }

        /// <summary>
        /// 增加过渡
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        public void AddTransition(BaseState start, BaseState end)
        {
            var transition = CreateInstance<Transition>();
            transition.name = start.name + "To" + end.name;
            transition.StartState = start;
            transition.EndState = end;
            start.Transitions.Add(transition);

            AssetDatabase.AddObjectToAsset(transition, start);
            AssetDatabase.SaveAssets();
        }

        /// <summary>
        /// 移除过渡
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        public void RemoveTransition(BaseState start, BaseState end)
        {
            Transition transition = start.Transitions.Find((t) => t.EndState == end);
            start.Transitions.Remove(transition);

            AssetDatabase.RemoveObjectFromAsset(transition);
            AssetDatabase.SaveAssets();
        }

        /// <summary>
        /// 开始执行状态机
        /// </summary>
        /// <param name="user"></param>
        public void OnEnter(BehaviorStateMachine user)
        {
            user.CurState = EnterState;
        }

        /// <summary>
        /// 运行状态机
        /// </summary>
        /// <param name="user"></param>
        public void OnUpdate(BehaviorStateMachine user)
        {
            if(TryGetNextState(user, out BaseState nextState))
            {
                SetState(user, nextState);
            }
            States[user.CurState].OnUpdate(user);
        }

        /// <summary>
        /// 获取下一个状态
        /// </summary>
        /// <returns>是否改变状态</returns>
        private bool TryGetNextState(BehaviorStateMachine user, out BaseState nextState)
        {
            //先检查任意状态过渡是否符合
            foreach(var transition in AnyState)
            {
                if (transition.Reason(user))
                {
                    nextState = transition.EndState;
                    return true;
                }
            }
            //再检查当前状态的过渡是否符合
            return States[user.CurState].TryGetNextState(user, out nextState);
        }

        /// <summary>
        /// 设置为指定状态
        /// </summary>
        /// <param name="user"></param>
        /// <param name="state"></param>
        private void SetState(BehaviorStateMachine user, BaseState state)
        {
            state.OnExit(user);
            user.CurState = States.IndexOf(state);
            state.OnEnter(user);
        }

        /// <summary>
        /// 伤害逻辑
        /// </summary>
        public void Hurt(BehaviorStateMachine user, float hurt, Status status)
        {
            States[user.CurState].Hurt(user, hurt, status);
        }

        public IProduct Clone()
        {
            return this;
        }
    }
}