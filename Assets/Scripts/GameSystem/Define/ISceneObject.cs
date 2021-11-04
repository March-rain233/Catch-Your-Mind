using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 需要受到SceneObjectManager管控生命周期的继承该接口
/// </summary>
public interface ISceneObject
{
    GameObject GetObject();
}
