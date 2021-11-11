using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPC
{
    [CreateAssetMenu(fileName = "ÂóµÂ×ÅÂ½×´Ì¬", menuName = "½ÇÉ«/×´Ì¬/ÂóµÂ×ÅÂ½")]
    public class MindLanding : BaseState
    {
        public override void OnEnter(BehaviorStateMachine user)
        {
            base.OnEnter(user);
            user.Animator.SetBool("IsLanding", true);
        }

        public override void OnExit(BehaviorStateMachine user)
        {
            base.OnExit(user);
            user.Animator.SetBool("IsLanding", false);
        }

        public override void OnUpdate(BehaviorStateMachine user)
        {
            
        }
    }
}
