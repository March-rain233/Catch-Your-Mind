using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPC
{

    public class DragWallNode : MindNode
    {
        /// <summary>
        /// �����ٶ�
        /// </summary>
        public float DownVector;

        protected override NodeStatus OnUpdate(BehaviorTreeRunner runner)
        {
            _rg.velocity = Vector2.down * DownVector;
            var pair = GameManager.Instance.ControlManager.KeyDic;
            //��-1 ��0 ��1
            int horizontal =
                System.Convert.ToInt32(UnityEngine.Input.GetKey(pair[KeyType.Right]))
                - System.Convert.ToInt32(UnityEngine.Input.GetKey(pair[KeyType.Left]));

            if(horizontal == -_model.FaceDir)
            {
                _model.FaceDir = horizontal;
                return NodeStatus.Success;
            }
            return NodeStatus.Running;
        }
    }
}
