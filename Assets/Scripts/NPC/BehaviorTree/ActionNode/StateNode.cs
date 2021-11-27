using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPC {

    /// <summary>
    /// 状态节点基类
    /// </summary>
    /// <remarks>
    /// 自动播放输入的动画
    /// </remarks>
    public abstract class StateNode : ActionNode
    {
        protected Animator _animator;
        protected Rigidbody2D _rg;
        protected NPC_Model _model;
        [SerializeField]
        protected string _triggerName;

        /// <summary>
        /// 清除所有的激活中的trigger缓存
        /// </summary>
        public void ResetAllTriggers(Animator animator)
        {
            AnimatorControllerParameter[] aps = animator.parameters;
            for (int i = 0; i < aps.Length; i++)
            {
                AnimatorControllerParameter paramItem = aps[i];
                if (paramItem.type == AnimatorControllerParameterType.Trigger)
                {
                    string triggerName = paramItem.name;
                    bool isActive = animator.GetBool(triggerName);
                    if (isActive)
                    {
                        animator.ResetTrigger(triggerName);
                    }
                }
            }
        }

        protected override void OnEnter(BehaviorTreeRunner runner)
        {
            _animator = runner.Variables["Animator"].Object as Animator;
            _rg = runner.Variables["Rigidbody"].Object as Rigidbody2D;
            _model = runner.Variables["Model"].Object as NPC_Model;
            ResetAllTriggers(_animator);
            _animator.SetTrigger(_triggerName);
        }

        protected override void OnExit(BehaviorTreeRunner runner)
        {
            _animator.ResetTrigger(_triggerName);
        }

        protected override void OnAbort(BehaviorTreeRunner runner)
        {
            OnExit(runner);
        }

        protected override void OnResume(BehaviorTreeRunner runner)
        {
            OnEnter(runner);
        }
    }
}
