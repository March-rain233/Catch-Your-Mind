using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Config
{
    [CreateAssetMenu(fileName = "场景配置汇总", menuName = "配置文件/场景配置汇总")]
    public class SceneInfoConfig : SerializedScriptableObject
    {
        public Dictionary<string, SceneInfo> ScenesObject;
    }
}
