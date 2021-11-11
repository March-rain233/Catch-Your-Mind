using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPC
{
    [CreateAssetMenu(fileName = "速度条件", menuName = "角色/条件/条件过渡")]
    public class VelocityCondition : BaseCondition
    {
        /// <summary>
        /// 检测的标准速度
        /// </summary>
        [SerializeField]
        private Vector2 _maxVelocity;

        /// <summary>
        /// 忽略X轴
        /// </summary>
        [SerializeField]
        private bool _ignoreX;
        /// <summary>
        /// 忽略Y轴
        /// </summary>
        [SerializeField]
        private bool _ignoreY;

        public override bool Reason(BehaviorStateMachine user)
        {
            var uv = user.Model.RigidBody.velocity;
            var mv = _maxVelocity;
            if(!_ignoreY && mv.y < 0) 
            {
                mv.y = -mv.y;
                uv.y = -uv.y;
            }
            if(!_ignoreX && mv.x < 0)
            {
                mv.x = -mv.x;
                uv.x = -uv.x;
            }

            if((_ignoreY || uv.y<=mv.y) && (_ignoreX || uv.x <= mv.x)) { return false; }
            return true;
        }
    }
}
