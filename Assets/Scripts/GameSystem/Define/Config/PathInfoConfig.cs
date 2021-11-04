using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Config
{
    [CreateAssetMenu(fileName = "路径配置汇总", menuName = "配置文件/路径设置汇总")]
    public class PathInfoConfig : SerializedScriptableObject
    {
        public Dictionary<ObjectType, PathInfo> Paths;
    }
}
