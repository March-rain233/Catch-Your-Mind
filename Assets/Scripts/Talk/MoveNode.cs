using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Dialogue
{

    public class MoveNode :ActionNode
    {
        public Vector2 Target;

        public int Time;

        private Animator Mind;

        bool end = false;

        protected override void OnEnter(DialogueTree tree)
        {
            base.OnEnter(tree);
            Mind = GameObject.Find("Mind").GetComponent<Animator>();
            Mind.SetTrigger("Run");
            Mind.SetFloat("FaceDirection", Mathf.Clamp(Target.x - Mind.transform.position.x, -1, 1));
            end = false;
            Mind.transform.DOMoveX(Target.x, Time).onComplete = () => end = true;
        }

        protected override NodeStatus OnUpdate(DialogueTree tree)
        {
            if (end) return NodeStatus.Success;
            return NodeStatus.Running;
        }
    }
}