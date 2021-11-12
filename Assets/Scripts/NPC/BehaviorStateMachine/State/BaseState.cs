using Item;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace NPC
{
    /// <summary>
    /// ��ɫ״̬����
    /// </summary>
    /// <remarks>
    /// �뽫�����ֶ���Ϊ���ɶ��Ա��������Լ���
    /// </remarks>
    public abstract class BaseState : ScriptableObject, INode
    {
        /// <summary>
        /// ��ͼ��λ��
        /// </summary>
        public Vector2 ViewPosition { get; set; }

        /// <summary>
        /// ��ʶ
        /// </summary>
        public string Guid { get; set; }

        /// <summary>
        /// ����
        /// </summary>
        public string Name { get => name; }

        public bool IsRoot => false;

        public bool IsLeaf => false;

        public Port.Capacity Output => Port.Capacity.Multi;

        public Port.Capacity Input => Port.Capacity.Multi;

        /// <summary>
        /// ����
        /// </summary>
        public List<Transition> Transitions = new List<Transition>();

        /// <summary>
        /// ��ȡ��һ��״̬
        /// </summary>
        /// <returns>�Ƿ�ı�״̬</returns>
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
        /// �߼�ִ�к���
        /// </summary>
        /// <returns>��һ״̬</returns>
        public abstract void OnUpdate(BehaviorStateMachine user);

        /// <summary>
        /// ������״̬ʱ
        /// </summary>
        public virtual void OnEnter(BehaviorStateMachine user)
        {
            Debug.Log($"����{this.name}");
        }

        /// <summary>
        /// ������״̬ʱ
        /// </summary>
        public virtual void OnExit(BehaviorStateMachine user)
        {
            Debug.Log($"�˳�{this.name}");
        }

        /// <summary>
        /// ���⵽�˺�ʱ�ļ����߼�
        /// </summary>
        /// <param name="user">������</param>
        /// <param name="hurt">����ֵ</param>
        /// <param name="status">����״̬</param>
        public virtual void Hurt(BehaviorStateMachine user, float hurt, Status status)
        {
        }

        public INode[] GetChildren()
        {
            throw new System.NotImplementedException();
        }
    }
}