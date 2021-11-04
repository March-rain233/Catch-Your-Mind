using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Config
{
    [CreateAssetMenu(fileName = "路径配置", menuName = "配置文件/路径设置")]
    public class PathInfo : SerializedScriptableObject
    {
        /// <summary>
        /// 支持的文件后缀名
        /// </summary>
        [SerializeField, LabelText("后缀名列表")]
        public static readonly string[] _extensionList =
        {
            "asset",
            "prefab",
            "png",
            "jpg",
            "mp3",
            "ogg",
            "wav"
        };

        [SerializeField]
        public Dictionary<string, string> PathDic;
        /// <summary>
        /// 文件所在目录
        /// </summary>
        [LabelText("文件所在目录")]
        public string CurPath;

        /// <summary>
        /// 将目录下所有文件重新读入
        /// </summary>
        [Button("重置")]
        public virtual void Reset()
        {
            string path = Application.dataPath + @"\Resources\" + CurPath;
            if (!Directory.Exists(path))
            {
                Debug.LogWarning($"{CurPath}不存在");
                return;
            }
            PathDic = new Dictionary<string, string>();

            //文件夹栈，通过迭代获取每一个子文件夹的文件
            Stack<string> directoryStack = new Stack<string>();
            directoryStack.Push(path);
            while (directoryStack.Count > 0)
            {
                path = directoryStack.Pop();
                Debug.Log($"当前处理文件夹：{path}");
                var dires = Directory.GetDirectories(path);
                foreach(var dire in dires)
                {
                    directoryStack.Push(dire);
                }
                for (int i = 0; i < _extensionList.Length; ++i)
                {
                    var files = Directory.GetFiles(path, "*." + _extensionList[i]);
                    //如果为图片，则把每一张图分别打包
                    if (_extensionList[i] == "png")
                    {
                        foreach (var f in files)
                        {
                            var spr = Resources.LoadAll<Sprite>(path.
                                Remove(0, (Application.dataPath + 
                                @"\Resources\").Length) + @"\" + Path.
                                GetFileNameWithoutExtension(f));
                            foreach(var s in spr)
                            {
                                string fileName = s.name;
                                string filePath = path.Remove(0, (Application.dataPath
                                    + @"\Resources\").Length) + @"\" + fileName;
                                PathDic.Add(fileName, filePath);
                                Debug.Log($"录入{fileName} 路径：{filePath}");
                            }
                        }
                    }
                    else
                    {
                        foreach (var f in files)
                        {
                            string fileName = Path.GetFileNameWithoutExtension(f);
                            string filePath = path.Remove(0, (Application.dataPath
                                + @"\Resources\").Length) + @"\" + fileName;
                            if(fileName == this.name)
                            {
                                continue;
                            }
                            PathDic.Add(fileName, filePath);
                            Debug.Log($"录入{fileName} 路径：{filePath}");
                        }
                    }
                }
            }
            Debug.Log("录入完成");
        }
    }
}