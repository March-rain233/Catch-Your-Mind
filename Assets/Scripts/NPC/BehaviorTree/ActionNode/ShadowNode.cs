using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace NPC
{
    /// <summary>
    /// ¿ØÖÆ²ÐÓ°
    /// </summary>
    public class ShadowNode : ActionNode
    {
        public bool IgnoreShdaowActive;
        public bool IgnoreShadowColor;
        public bool IgnoreExistTime;
        public bool IgnoreInterval;

        [HideIf("IgnoreShdaowActive")]
        public bool ShadowActive;

        [HideIf("IgnoreShadowColor")]
        public Color ShadowColor;

        [HideIf("IgnoreExistTime")]
        public float ExistTime;

        [HideIf("IgnoreInterval")]
        public float Interval;

        protected override void OnAbort(BehaviorTreeRunner runner)
        {

        }

        protected override void OnEnter(BehaviorTreeRunner runner)
        {

        }

        protected override void OnExit(BehaviorTreeRunner runner)
        {

        }

        protected override void OnResume(BehaviorTreeRunner runner)
        {

        }

        protected override NodeStatus OnUpdate(BehaviorTreeRunner runner)
        {
            var shadow = runner.Variables["Shadow"].Object as ShadowController;
            if(!IgnoreExistTime) shadow.ExistTime = ExistTime;
            if(!IgnoreShadowColor)shadow.ShadowColor = ShadowColor;
            if(!IgnoreShdaowActive) shadow.IsCreate = ShadowActive;
            if (!IgnoreInterval) shadow.Interval = Interval;
            return NodeStatus.Success;
        }
    }
}
