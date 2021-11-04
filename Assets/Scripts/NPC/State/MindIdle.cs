using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPC
{
    [CreateAssetMenu(fileName = "��´���״̬", menuName = "��ɫ/״̬/��´���")]
    public class MindIdle : BaseState
    {
        public override void OnEnter(BehaviorStateMachine user)
        {
            base.OnEnter(user);
            user.Model.Anim.SetBool("IsIdle", true);
            user.Model.RigidBody.velocity = Vector2.zero;
        }

        public override void OnExit(BehaviorStateMachine user)
        {
            base.OnExit(user);
            user.Model.Anim.SetBool("IsIdle", false);
        }

        public override void OnUpdate(BehaviorStateMachine user)
        {
            
        }
    }
}
