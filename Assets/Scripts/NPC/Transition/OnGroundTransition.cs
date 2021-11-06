using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPC
{
    [CreateAssetMenu(fileName = "地面检测过渡", menuName = "角色/过渡/地面检测过渡")]
    public class OnGroundTransition : BaseTransition
    {
        public override bool Reason(BehaviorStateMachine user)
        {
            return user.Model.IsOnGround;
        }
    }
}
