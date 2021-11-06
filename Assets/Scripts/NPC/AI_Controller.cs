using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Item;

namespace NPC
{
    /// <summary>
    /// AI����
    /// </summary>
    /// <remarks>
    /// ִ��NPC����Ϊ�߼���ʵ������״̬��
    /// </remarks>
    [CreateAssetMenu(fileName = "��Ϊ������", menuName = "��ɫ/��ɫ��Ϊ������")]
    public class AI_Controller : SerializedScriptableObject, IProduct
    {
        /// <summary>
        /// ״̬�ֵ�
        /// </summary>
        [SerializeField]
        private Dictionary<string, BaseState> _states;

        /// <summary>
        /// ״̬ͼ
        /// </summary>
        [SerializeField]
        private Dictionary<string, TransitionInfo[]> _graph;

        /// <summary>
        /// �κ�״̬����
        /// </summary>
        [SerializeField]
        private TransitionInfo[] _anyState;

        /// <summary>
        /// �����ʼ״̬����
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
        /// ��ȡ��һ��״̬
        /// </summary>
        /// <returns>�Ƿ�ı�״̬</returns>
        private bool TryGetNextState(BehaviorStateMachine user, out string nextState)
        {
            //�ȼ������״̬�����Ƿ����
            TransitionInfo[] map = _anyState;
            for (int i = 0; i < map.Length; ++i)
            {
                if (map[i].Transition.Reason(user))
                {
                    nextState = map[i].Target;
                    return true;
                }
            }
            //�ټ�鵱ǰ״̬�Ĺ����Ƿ����
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
        /// ����Ϊָ��״̬
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
        /// �˺��߼�
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
        /// ������Ϣ
        /// </summary>
        [SerializeField, System.Serializable]
        private struct TransitionInfo
        {
            /// <summary>
            /// ����
            /// </summary>
            public BaseTransition Transition;

            /// <summary>
            /// Ŀ��״̬
            /// </summary>
            public string Target;
        }
    }
}