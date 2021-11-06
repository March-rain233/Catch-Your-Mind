using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPC
{
    [CreateAssetMenu(fileName = "动画时间过渡", menuName = "角色/过渡/动画时间过渡")]
    public class AnimationTransition : BaseTransition
    {
        /// <summary>
        /// 检测的动画状态的名称
        /// </summary>
        [SerializeField]
        private string _animationName;

        /// <summary>
        /// 检测的动画状态的播放进度
        /// </summary>
        [SerializeField]
        private float _animationTime;

        public override bool Reason(BehaviorStateMachine user)
        {
            var info = user.Animator.GetCurrentAnimatorStateInfo(0);
            if(info.IsName(_animationName) && info.normalizedTime >= _animationTime)
            {
                return true;
            }
            return false;
        }
    }
}
