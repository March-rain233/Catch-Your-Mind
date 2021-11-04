using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Config
{
    /// <summary>
    /// 场景物品配置文件
    /// </summary>
    [CreateAssetMenu(fileName = "场景配置", menuName = "配置文件/场景配置")]
    public class SceneInfo : SerializedScriptableObject
    {
        public Save.ObjectInfo[] Objects;
        public Dictionary<string, Vector2> Positions;
        public string Scene;
        public string BGM;
    }
}
