using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPC
{

    public class FallNode : MindNode
    {
        [SerializeField]
        private float _moveScale;

        [SerializeField]
        private float _exGravity;

        protected override NodeStatus OnUpdate(BehaviorTreeRunner runner)
        {
            _rg.AddForce(Vector2.down * _exGravity);

            var pair = GameManager.Instance.ControlManager.KeyDic;
            //左-1 中0 右1
            int horizontal =
                System.Convert.ToInt32(UnityEngine.Input.GetKey(pair[KeyType.Right]))
                - System.Convert.ToInt32(UnityEngine.Input.GetKey(pair[KeyType.Left]));

            //下-1 中0 上1
            bool jump = UnityEngine.Input.GetKeyDown(pair[KeyType.Jump]);
            //左-1 中0 右1

            if (horizontal == 0) { return NodeStatus.Running; }
            if (horizontal == 1)
            {
                _model.FaceDir = 1;
            }
            else
            {
                _model.FaceDir = -1;
            }
            if (_rg.velocity.x * _model.FaceDir < _moveScale * _model.Speed)
            {
                _rg.velocity = new Vector2(horizontal * _moveScale * _model.Speed, _rg.velocity.y);
            }
            return NodeStatus.Running;
        }
    }
}
