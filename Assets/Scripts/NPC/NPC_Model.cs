using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;


namespace NPC
{
    /// <summary>
    /// 角色组件的容器
    /// </summary>
    /// <remarks>
    /// 实际上是数据模型，通过改变数据状态来改变其他组件的行为模式
    /// </remarks>
    [Serializable]
    public class NPC_Model : MonoBehaviour, IBaseModel
    {
        /// <summary>
        /// 地面所在层级
        /// </summary>
        [SerializeField]
        private LayerMask _groundLayer;

        /// <summary>
        /// 角色绑定的刚体
        /// </summary>
        [SerializeField]
        public Rigidbody2D RigidBody
        {
            get
            {
                if(_rigidbody == null)
                {
                    _rigidbody = GetComponent<Rigidbody2D>();
                }
                return _rigidbody;
            }
        }
        private Rigidbody2D _rigidbody;

        /// <summary>
        /// 角色的位置
        /// </summary>
        [SerializeField]
        public Transform Transform 
        {
            get
            {
                return transform;
            }
        }

        /// <summary>
        /// 角色绑定的动画器
        /// </summary>
        [SerializeField]
        public Animator Animator
        {
            get
            {
                if(_animator == null)
                {
                    _animator = GetComponent<Animator>();
                }
                return _animator;
            }
        }
        private Animator _animator;

        /// <summary>
        /// 当前的角色朝向
        /// </summary>
        /// <remarks>
        /// 0-左 1-右
        /// </remarks>
        public float FaceDir
        {
            get { return _faceDir; }
            internal set { _faceDir = value; }
        }
        [SerializeField]
        private float _faceDir;

        /// <summary>
        /// 角色的属性
        /// </summary>
        [SerializeField]
        protected Breed _breed;

        /// <summary>
        /// 角色实际攻击力
        /// </summary>
        /// <remarks>
        /// 算法为基础攻击力+额外攻击力（如百分比加成和固定额度加成）
        /// </remarks>
        public float Attack
        {
            get
            {
                return (_breed.BaseAttack + Equip.GetValue("Attack")) *
                    Equip.GetRate("Attack");
            }
        }

        /// <summary>
        /// 角色实际防御力
        /// </summary>
        /// <remarks>
        /// 算法为基础防御力+额外防御力（如百分比加成和固定额度加成）
        /// </remarks>
        public float Defense
        {
            get
            {
                return (_breed.BaseDefense + Equip.GetValue("Defense")) *
                    Equip.GetRate("Defense");
            }
        }

        /// <summary>
        /// 角色实际速度
        /// </summary>
        /// <remarks>
        /// 算法为基础速度+额外速度（如百分比加成和固定额度加成）
        /// </remarks>
        public float Speed
        {
            get
            {
                return (_breed.BaseSpeed + Equip.GetValue("Speed")) * Equip.GetRate("Speed");
            }
        }

        /// <summary>
        /// 角色跳跃高度
        /// </summary>
        public float JumpHeight
        {
            get { return _breed.JumpHeight; }
        }

        /// <summary>
        /// 角色最大血量
        /// </summary>
        /// <remarks>
        /// 算法为基础血量+额外血量（如百分比加成和固定额度加成）
        /// </remarks>
        public float MaxBlood
        {
            get
            {
                return (_breed.BaseBlood + Equip.GetValue("Blood")) *
                    Equip.GetRate("Blood");
            }
        }

        /// <summary>
        /// 角色最大能量
        /// </summary>
        public float MaxEnergy
        {
            get;
            private set;
        }

        private float _blood;
        /// <summary>
        /// 角色当前血量
        /// </summary>
        public float Blood
        {
            get
            {
                return _blood;
            }
            set
            {
                _blood = value;
                NotifyEvent?.Invoke("Blood", _blood);
            }
        }

        private float _energy;
        /// <summary>
        /// 角色当前能量
        /// </summary>
        public float Energy
        {
            get
            {
                return _energy;
            }
            set
            {
                _energy = value;
                NotifyEvent?.Invoke("Energy", _energy);
            }
        }

        /// <summary>
        /// 角色是否站在地面上
        /// </summary>
        public bool IsOnGround
        {
            get
            {
                return _isOnGround;
            }
        }
        [SerializeField]
        private bool _isOnGround;

        /// <summary>
        /// 角色装备
        /// </summary>
        public NPC_Equip Equip
        {
            get
            {
                return _equip;
            }
            protected set
            {
                _equip = value;
            }
        }
        [SerializeField]
        private NPC_Equip _equip = new NPC_Equip();

        private void Update()
        {
            var hit = Physics2D.OverlapCircle(transform.position, 0.1f, _groundLayer);
            if (hit != null) { _isOnGround = true; }
            else { _isOnGround = false; }
            Animator.SetFloat("FaceDirection", FaceDir);
        }

        public event Action<string, object> NotifyEvent;

        public void Init(Breed breed, NPC_Equip equip, float blood = -1, float energy = -1)
        {
            _breed = breed;
            Equip = equip;
            FaceDir = 1;
            Blood = blood == -1 ? breed.BaseBlood : blood;
            Energy = energy == -1 ? breed.BaseEnergy : energy;
        }
    }
}
