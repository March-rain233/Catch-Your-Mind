//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using NPC;
//using Newtonsoft.Json;

///// <summary>
///// 角色工厂
///// </summary>
//public class NPC_Factory
//{
//    /// <summary>
//    /// 种类对象池
//    /// </summary>
//    private Dictionary<string, Breed> _breedPool;

//    /// <summary>
//    /// 预制体对象池
//    /// </summary>
//    private Dictionary<string, GameObject> _prefabsPool;

//    /// <summary>
//    /// 种类的文件地址
//    /// </summary>
//    private Save.PathConfig _breedPath;

//    public NPC_Factory()
//    {
//        string path = "config/PathConfig";
//        conf
//    }

//    /// <summary>
//    /// 获取种类
//    /// </summary>
//    /// <param name="name"></param>
//    /// <returns></returns>
//    public Breed GetBreed(string name)
//    {
//        //检查对象池内是否已存在对象
//        if(_breedPool.TryGetValue(name, out Breed res))
//        {
//            return res;
//        }

//        if(!_breedPath.TryGetValue(name, out string path))
//        {
//            Debug.LogWarning($"种族文件：{name} 缺失");
//            return default;
//        }

//        res = Resources.Load<Breed>(path);

//        //将种族存入对象池
//        PushBreed(res);
//        return res;
//    }

//    /// <summary>
//    /// 获取角色
//    /// </summary>
//    public NPC_Model GetNpc(Save.NPC_Save save)
//    {
//        //载入预制体并附加组件

//    }

//    private void PushBreed(Breed breed)
//    {
//        _breedPool[breed.Name] = breed;
//    }
//}
