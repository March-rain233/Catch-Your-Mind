using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPC
{
    /// <summary>
    /// 种类信息
    /// </summary>
    [CreateAssetMenu(fileName = "Breed", menuName = "种族")]
    [SerializeField, System.Serializable]
    public class Breed : SerializedScriptableObject, IProduct
    {
        public Breed Prototype;
        public string Name;
        [SerializeField]
        private float _baseSpeed;
        public float BaseSpeed
        {
            get
            {
                if (_baseSpeed < 0)
                {
                    return Prototype.BaseSpeed;
                }
                return _baseSpeed;
            }
        }
        [SerializeField]
        private float _jumpHeight;
        public float JumpHeight
        {
            get
            {
                if (_jumpHeight < 0)
                {
                    return Prototype.JumpHeight;
                }
                return _jumpHeight;
            }
        }
        [SerializeField]
        private float _baseBlood;
        public float BaseBlood
        {
            get
            {
                if (_baseBlood < 0)
                {
                    return Prototype.BaseBlood;
                }
                return _baseBlood;
            }
        }
        [SerializeField]
        private float _baseAttack;
        public float BaseAttack
        {
            get
            {
                if (_baseAttack < 0)
                {
                    return Prototype.BaseAttack;
                }
                return _baseAttack;
            }
        }
        [SerializeField]
        private float _baseDefense;
        public float BaseDefense
        {
            get
            {
                if (_baseDefense < 0)
                {
                    return Prototype.BaseDefense;
                }
                return _baseDefense;
            }
        }
        [SerializeField]
        private float _baseEnergy;
        public float BaseEnergy
        {
            get
            {
                if (_baseEnergy < 0)
                {
                    return Prototype.BaseEnergy;
                }
                return _baseEnergy;
            }
        }
        [SerializeField]
        private float _baseEnergyRecoverSpeed;
        public float BaseEnergyRecoverSpeed
        {
            get
            {
                if (_baseEnergyRecoverSpeed < 0)
                {
                    return Prototype.BaseEnergyRecoverSpeed;
                }
                return _baseEnergyRecoverSpeed;
            }
        }

        public IProduct Clone()
        {
            return this;
        }
    }
}
