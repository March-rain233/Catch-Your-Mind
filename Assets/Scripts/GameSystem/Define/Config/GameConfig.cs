using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Config
{
    /// <summary>
    /// 全局游戏设置
    /// </summary>
    [CreateAssetMenu(fileName = "全局配置", menuName = "配置文件/全局配置")]
    public class GameConfig : SerializedScriptableObject
    {
        public PathInfoConfig PathConfig;
        public SceneInfoConfig SceneInfoConfig;
    }
}