using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameAsset
{
    /// <summary>
    /// 对话文件
    /// </summary>
    public class Dialogue : SerializedScriptableObject
    {
        [System.Serializable]
        private class DialogueCell
        {
            /// <summary>
            /// 对话框内显示的名字
            /// </summary>
            public string Name;

            /// <summary>
            /// 对话框内的文本
            /// </summary>
            [TextArea]
            public string Text;

            /// <summary>
            /// 对话框所对应的游戏物体的名字
            /// </summary>
            public string ObjectName;
        }


    }
}
