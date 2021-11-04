using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Sirenix.OdinInspector;
using Map;
using Save;

/// <summary>
/// 负责场景的加载管理
/// </summary>
public class MapManager
{
    /// <summary>
    /// 载入场景
    /// </summary>
    public void LoadScene(string scene, System.Action loadCallBack)
    {
        //监视异步加载进度
        CoroutineManager.Instance.StartCoroutine(LoadAsyn
            (SceneManager.LoadSceneAsync(scene), loadCallBack));
    }

    /// <summary>
    /// 监视加载进度，加载完成后触发事件
    /// </summary>
    private IEnumerator LoadAsyn(AsyncOperation asyncScene, System.Action loadCallBack)
    {
        Debug.Log("正在异步载入场景……");
        yield return asyncScene;
        Debug.Log("载入结束");
        loadCallBack.Invoke();
    }

}
