using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameAsset
{
    public class Dialogue : SerializedScriptableObject
    {
        [SerializeField]
        public Dictionary<string, BaseAssertion> Assertions;

        public TextAsset Text;
    }
}
