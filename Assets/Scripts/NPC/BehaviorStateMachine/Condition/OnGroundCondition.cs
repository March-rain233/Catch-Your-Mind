using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPC
{
    [CreateAssetMenu(fileName = "地面检测条件", menuName = "角色/条件/地面检测条件")]
    public class OnGroundCondition : BaseCondition
    {
        public override bool Reason(BehaviorStateMachine user)
        {
            return user.Model.IsOnGround;
        }
    }
}
