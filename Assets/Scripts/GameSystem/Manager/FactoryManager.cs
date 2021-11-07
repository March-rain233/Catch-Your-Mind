using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Sirenix.OdinInspector;
using Map;
using NPC;
using Save;

/// <summary>
/// 游戏数据存储处理，以及游戏运行时原始数据获取
/// </summary>
public class FactoryManager
{
    private Dictionary<ObjectType, ObjectFactoryWithPool> _factorys = new Dictionary<ObjectType, ObjectFactoryWithPool>();

    public T LoadResource<T>(ObjectType type, string name) where T : Object
    {
        Debug.Log($"正在载入{type} : {name}");
        return Resources.Load<T>(GameManager.Instance.GameConfig.
            PathConfig.Paths[type].PathDic[name]);
    }

    /// <summary>
    /// 把数据作为文本文件储存
    /// </summary>
    /// <typeparam name="T">存储数据的数据类型</typeparam>
    /// <param name="data">存储的数据</param>
    /// <param name="filePath">文件路径，格式应为"/文件夹/文件名</param>
    /// <param name="mode">
    /// <para>mode1 路径为PersistentDataPath(默认)用于存取存档文件</para>
    /// <para>mode2 路径为streamingAssetsPath用于存取打包后只读的列表数据</para>
    /// </param>
    public void Save<T>([SerializeField] T data, string filePath, int mode = 1)
    {
        ///对输入的文件地址进行处理
        switch (mode)
        {
            case 1:
                filePath = Application.persistentDataPath + @filePath;
                break;
            case 2:
                filePath = Application.streamingAssetsPath + @filePath;
                break;
            default:
                return;
        }
        string s = filePath.Substring(0, filePath.LastIndexOf('/'));

        Directory.CreateDirectory(s);
        StreamWriter file = new StreamWriter(filePath);
        string json = JsonUtility.ToJson(data, true);
        Debug.Log(json);
        file.Write(json);
        file.Close();
    }
    /// <summary>
    /// 读入数据
    /// </summary>
    /// <typeparam name="T">读入数据的数据类型</typeparam>
    /// <param name="data">读入数据的储存位置</param>
    /// <param name="filePath">文件路径，格式应为"/文件夹/文件名</param>
    /// <param name="mode">
    /// <para>mode1 路径为PersistentDataPath(默认)用于存取存档文件</para>
    /// <para>mode2 路径为streamingAssetsPath用于存取打包后只读的列表数据</para>
    /// </param>
    /// <returns>是否成功读取</returns>
    public bool Load<T>(out T data, string filePath, int mode = 1)
    {
        ///对输入的文件地址进行处理
        switch (mode)
        {
            case 1:
                filePath = Application.persistentDataPath + @filePath;
                break;
            case 2:
                filePath = Application.streamingAssetsPath + @filePath;
                break;
            default:
                data = default(T);
                return false;
        }

        if (!File.Exists(filePath))
        {
            data = default(T);
            return false;
        }
        StreamReader file = new StreamReader(filePath);
        string json = file.ReadToEnd();
        data = default(T);
        JsonUtility.FromJsonOverwrite(json, data);
        return true;
    }

    /// <summary>
    /// 用于保存字典
    /// </summary>
    /// <see cref="Save{T}(T, string, int)"/>
    public void SaveDic<TKey, TVal>([SerializeField] Dictionary<TKey,TVal> data, string filePath, int mode = 1)
    {
        Save(new MyTools.Serialization<TKey, TVal>(data), filePath, mode);
    }

    /// <summary>
    /// 用于读取字典
    /// </summary>
    /// <see cref="Load{T}(out T, string, int)"/>
    public bool LoadDic<TKey, TVal>(out Dictionary<TKey, TVal> data, string filePath, int mode = 1)
    {
        bool t = Load(out MyTools.Serialization<TKey, TVal> temp, filePath);
        if (!t)
        {
            data = new Dictionary<TKey, TVal>();
            return false;
        }
        temp.OnAfterDeserialize();
        data = temp.ToDictionary();
        return t;
    }

    /// <summary>
    /// 添加工厂
    /// </summary>
    /// <param name="type"></param>
    /// <param name="factory"></param>
    public void AddFactory(ObjectType type, ObjectFactoryWithPool factory)
    {
        _factorys.Add(type, factory);
    }

    /// <summary>
    /// 移除工厂
    /// </summary>
    /// <param name="type"></param>
    public void RemoveFactory(ObjectType type)
    {
        _factorys.Remove(type);
    }

    /// <summary>
    /// 创建对象
    /// </summary>
    /// <param name="type"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public IProduct Create(ObjectType type, string name)
    {
        return _factorys[type].Create(name);
    }
}
