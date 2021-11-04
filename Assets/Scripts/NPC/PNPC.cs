using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPC
{
    /// <summary>
    /// ½ÇÉ«²úÆ·
    /// </summary>
    public class PNPC : IProduct, ISceneObject
    {
        public BehaviorStateMachine StateMachine;
        public NPC_Model Model;

        public IProduct Clone()
        {
            var clone = GameObject.Instantiate<GameObject>
                (StateMachine.gameObject);
            clone.name = clone.name.Replace("(Clone)", "");
            PNPC p = new PNPC();
            p.StateMachine = clone.GetComponent<BehaviorStateMachine>();
            p.Model = clone.GetComponent<NPC_Model>();
            return p; 
        }

        public GameObject GetObject()
        {
            return StateMachine.gameObject;
        }
    }
}
