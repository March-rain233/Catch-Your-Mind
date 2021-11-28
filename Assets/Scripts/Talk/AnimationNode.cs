using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogue
{

    public class AnimationNode : ActionNode
    {
        [System.Serializable]
        public struct AnimationInfo
        {
            public string ObjectName;
            public string TriggerName;
            public float Time;
            /// <summary>
            /// 是否等待该动画播放完毕
            /// </summary>
            public bool IsWatiting;
        }
        public List<AnimationInfo> AnimationQueue;

        private Animator _animator;

        private int _current;

        protected override void OnEnter(DialogueTree tree)
        {
            base.OnEnter(tree);

            _current = 0;
            SetAnimator(tree);
        }

        protected override void OnExit(DialogueTree tree)
        {
            base.OnExit(tree);
            _animator = null;
            _current = 0;
        }

        private void SetAnimator(DialogueTree tree)
        {
            GameObject o;
            if (tree.Variables.ContainsKey(AnimationQueue[_current].ObjectName))
            {
                o = tree.Variables[AnimationQueue[_current].ObjectName].Object as GameObject;
            }
            else
            {
                o = GameObject.Find(AnimationQueue[_current].ObjectName);
                tree.Variables.Add(AnimationQueue[_current].ObjectName, new EventCenter.EventArgs() { Object = o });
            }
            _animator = o.GetComponent<Animator>();
            _animator.SetTrigger(AnimationQueue[_current].TriggerName);
        }

        protected override NodeStatus OnUpdate(DialogueTree tree)
        {
            var state = _animator.GetCurrentAnimatorStateInfo(0);
            if (state.IsName(AnimationQueue[_current].TriggerName)
                && (state.normalizedTime >= AnimationQueue[_current].Time 
                || !AnimationQueue[_current].IsWatiting))
            {
                if(_current++ >= AnimationQueue.Count - 1)
                {
                    return NodeStatus.Success;
                }
                SetAnimator(tree);
            }
            Debug.Log(AnimationQueue[_current].TriggerName + _animator.gameObject.name);

            return NodeStatus.Running;
        }
    }
}
