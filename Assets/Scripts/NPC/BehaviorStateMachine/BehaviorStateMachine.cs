using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Item;

namespace NPC
{
    /// <summary>
    /// 角色ai控制器
    /// </summary>
    public class BehaviorStateMachine : MonoBehaviour, IHurt
    {
        /// <summary>
        /// 绑定的NPC模型
        /// </summary>
        [SerializeField]
        public NPC_Model Model
        {
            get
            {
                if(_model == null)
                {
                    _model = GetComponent<NPC_Model>();
                }
                return _model;
            }
        }
        private NPC_Model _model;

        /// <summary>
        /// 使用的状态机配置文件
        /// </summary>
        [SerializeField, LabelText("控制器")]
        private StateMachineController _controller;

        public Animator Animator
        {
            get
            {
                return Model.Animator;
            }
        }

        /// <summary>
        /// 当前状态
        /// </summary>
        public int CurState
        {
            get
            {
                return _curState;
            }
            internal set
            {
                _curState = value;
                RunTime = 0;
            }
        }
        [SerializeField, SetProperty("CurState")]
        private int _curState;

        /// <summary>
        /// 当前状态开始运行的时间（为游戏启动以来的时间）
        /// </summary>
        public float StartTime
        {
            get
            {
                return _startTime;
            }
            set
            {
                _startTime = Time.time;
                RunTime = 0;
            }
        }
        private float _startTime;

        /// <summary>
        /// 当前状态的总计运行时间（不包括暂停）
        /// </summary>
        public float RunTime
        {
            get
            {
                if (IsActive)
                {
                    RunTime = Time.time - _runTime.LastSetTime + _runTime.RunTime;
                }
                return _runTime.RunTime;
            }
            private set
            {
                _runTime.RunTime = value;
                _runTime.LastSetTime = Time.time;
            }
        }
        private RunTimer _runTime;

        /// <summary>
        /// ai是否在运行
        /// </summary>
        public bool IsActive
        {
            get
            {
                return _isActive;
            }
            set
            {
                if(!value)
                {
                    //当禁用时计算运行时间，并停止累加
                    RunTime = RunTime;
                }
                else if (value ^ _isActive)
                {
                    //当从禁用转为运行时，把运行时间的修改时间修正
                    _runTime.LastSetTime = Time.time;
                }
                _isActive = value;
            }
        }
        private bool _isActive = true;

        public virtual void Init(StateMachineController control)
        {
            _controller = control;
            _curState = -1;
            StartTime = RunTime = 0;
        }

        protected virtual void Start()
        {
            _controller.OnEnter(this);   
        }

        /// <remarks>
        /// 先进行状态机的更新，再处理命令
        /// </remarks>
        void Update()
        {
            if (!IsActive)
            {
                return;
            }
            _controller.OnUpdate(this);
        }

        public void Hurt(float hurt, Status status = Status.Common)
        {
            _controller.Hurt(this, hurt, status);
        }

        private struct RunTimer
        {
            public float RunTime;
            public float LastSetTime;
        }
    }
}
