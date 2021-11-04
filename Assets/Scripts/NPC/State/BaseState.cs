using Item;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPC
{
    /// <summary>
    /// ��ɫ״̬����
    /// </summary>
    /// <remarks>
    /// �뽫�����ֶ���Ϊ���ɶ��Ա��������Լ���
    /// </remarks>
    public abstract class BaseState : ScriptableObject
    {
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
            Debug.Log($"����{this.name}״̬");
            user.Animator.SetBool("FaceDirection", user.Model.FaceDir);
        }

        /// <summary>
        /// ������״̬ʱ
        /// </summary>
        public virtual void OnExit(BehaviorStateMachine user)
        {
            Debug.Log($"�˳�{this.name}״̬");
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
    }
}