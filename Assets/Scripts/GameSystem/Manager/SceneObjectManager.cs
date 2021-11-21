using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 管控场景物品的生命周期
/// </summary>
public class SceneObjectManager : MonoBehaviour
{
    /// <summary>
    /// 当前场景的物品信息
    /// </summary>
    private Save.ObjectInfo[] CurScene
    {
        get
        {
            return GameManager.Instance.CurrentScene.Objects;
        }
    }

    /// <summary>
    /// 还未载入的对象
    /// </summary>
    [SerializeField]
    private List<Save.ObjectInfo> _unloadObjects;
    /// <summary>
    /// 正在运行的对象
    /// </summary>
    [SerializeField]
    private List<MySceneObject> _activeObjects;
    /// <summary>
    /// 被禁用的对象
    /// </summary>
    [SerializeField]
    private List<MySceneObject> _unactiveObjects;

    public void Init()
    {
        GameManager.Instance.OnLoadingPosition += (Map.MapPosition t) =>
        {
            CheckReasons();
        };
        GameManager.Instance.EventCenter.EventChanged += (string eventName, EventCenter.EventArgs eventArgs) =>
        {
            CheckReasons();
        };
        _activeObjects = new List<MySceneObject>();
        _unactiveObjects = new List<MySceneObject>();
        _unloadObjects = new List<Save.ObjectInfo>();
    }

    /// <summary>
    /// 检查物品生命周期状态
    /// </summary>
    public void CheckReasons()
    {
        CheckToload();
        CheckToActive();
        CheckToUnactive();
        CheckToDestory();
    }

    /// <summary>
    /// 创建对象
    /// </summary>
    /// <param name="info"></param>
    private MySceneObject CreateObject(Save.ObjectInfo info)
    {
        MySceneObject res = new MySceneObject();

        res.Object = (GameManager.Instance.FactoryManager.
        Create(info.Type, info.ResourceName) as ISceneObject).
        GetObject();
        res.Object.transform.position = res.Info.Position;
        res.Info = info;
        return res;
    }

    /// <summary>
    /// 检查是否有可生成对象
    /// </summary>
    private void CheckToload()
    {
        for(int i = _unloadObjects.Count - 1; i >= 0; --i)
        {
            if (_unloadObjects[i].CreatReason.Value)
            {
                var t = CreateObject(_unloadObjects[i]);
                _unloadObjects.RemoveAt(i);
                _activeObjects.Add(t);
            }
        }
    }

    /// <summary>
    /// 检查是否有可激活对象
    /// </summary>
    private void CheckToActive()
    {
        for(int i = _unactiveObjects.Count - 1; i >= 0; --i)
        {
            if (_unactiveObjects[i].Info.EnableReason.Value)
            {
                _unactiveObjects[i].Object.SetActive(true);
                _activeObjects.Add(_unactiveObjects[i]);
                _unactiveObjects.RemoveAt(i);
            }
        }
    }

    /// <summary>
    /// 检查是否有需要禁用对象
    /// </summary>
    private void CheckToUnactive()
    {
        for (int i = _activeObjects.Count - 1; i >= 0; --i)
        {
            if (_activeObjects[i].Info.DisableReason.Value)
            {
                _activeObjects[i].Object.SetActive(false);
                _unactiveObjects.Add(_activeObjects[i]);
                _activeObjects.RemoveAt(i);
            }
        }
    }

    /// <summary>
    /// 检查是否有需要销毁对象
    /// </summary>
    private void CheckToDestory()
    {
        for (int i = _unactiveObjects.Count - 1; i >= 0; --i)
        {
            if (_unactiveObjects[i].Info.DestoryReason.Value)
            {
                Destroy(_unactiveObjects[i].Object);
                _unloadObjects.Add(_unactiveObjects[i].Info);
                _unactiveObjects.RemoveAt(i);
            }
        }
        for (int i = _activeObjects.Count - 1; i >= 0; --i)
        {
            if (_activeObjects[i].Info.DestoryReason.Value)
            {
                Destroy(_activeObjects[i].Object);
                _unloadObjects.Add(_activeObjects[i].Info);
                _activeObjects.RemoveAt(i);
            }
        }
    }

    [SerializeField]
    private class MySceneObject
    {
        public Save.ObjectInfo Info;
        public GameObject Object;
    }
}
