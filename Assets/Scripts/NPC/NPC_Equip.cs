using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPC
{
    [System.Serializable]
    public class NPC_Equip
    {
        public Item.Weapon LeftHand;
        public Item.Weapon RightHand;
        public Item.Equipment Head;
        public Item.Equipment Body;
        public Item.Equipment Pants;
        public Item.Equipment Feet;

        /// <summary>
        /// 获取基础数值
        /// </summary>
        public float GetValue(string type)
        {
            switch (type)
            {
                case "Attack":
                    return (Head == null ? 0 : Head.Attack)
                        + (Body == null ? 0 : Body.Attack)
                        + (Pants == null ? 0 : Pants.Attack)
                        + (Feet == null ? 0 : Feet.Attack);
                case "Defense":
                    return (Head == null ? 0 : Head.Defense)
                        + (Body == null ? 0 : Body.Defense)
                        + (Pants == null ? 0 : Pants.Defense)
                        + (Feet == null ? 0 : Feet.Defense);
                case "Blood":
                    return (Head == null ? 0 : Head.Blood)
                        + (Body == null ? 0 : Body.Blood)
                        + (Pants == null ? 0 : Pants.Blood)
                        + (Feet == null ? 0 : Feet.Blood);
                case "Speed":
                    return (Head == null ? 0 : Head.Speed)
                        + (Body == null ? 0 : Body.Speed)
                        + (Pants == null ? 0 : Pants.Speed)
                        + (Feet == null ? 0 : Feet.Speed);
                default:
                    return 0;
            }
        }

        /// <summary>
        /// 获取加成倍率
        /// </summary>
        /// <returns></returns>
        public float GetRate(string type)
        {
            float rate;
            switch (type)
            {
                case "Attack":
                    rate = (Head == null ? 0 : Head.AttackRate)
                        + (Body == null ? 0 : Body.AttackRate)
                        + (Pants == null ? 0 : Pants.AttackRate)
                        + (Feet == null ? 0 : Feet.AttackRate);
                    return rate == 0 ? 1 : rate;
                case "Defense":
                    rate = (Head == null ? 0 : Head.DefenseRate)
                        + (Body == null ? 0 : Body.DefenseRate)
                        + (Pants == null ? 0 : Pants.DefenseRate)
                        + (Feet == null ? 0 : Feet.DefenseRate);
                    return rate == 0 ? 1 : rate;
                case "Blood":
                    rate = (Head == null ? 0 : Head.BloodRate)
                        + (Body == null ? 0 : Body.BloodRate)
                        + (Pants == null ? 0 : Pants.BloodRate)
                        + (Feet == null ? 0 : Feet.BloodRate);
                    return rate == 0 ? 1 : rate;
                case "Speed":
                    rate = (Head == null ? 0 : Head.SpeedRate)
                        + (Body == null ? 0 : Body.SpeedRate)
                        + (Pants == null ? 0 : Pants.SpeedRate)
                        + (Feet == null ? 0 : Feet.SpeedRate);
                    return rate == 0 ? 1 : rate;
                default:
                    return 1;
            }
        }
    }
}