using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Sirenix.OdinInspector;
using Save;
using Map;
using NPC;
using Config;
using UnityEngine.SceneManagement;

/// <summary>
/// 管理管理类，作为中介者把操作分发给其他管理类
/// </summary>
/// <remarks>
/// 如果此类的功能可以满足需求请不要调用其他管理器
/// </remarks>
public abstract class BaseGameManager<T> : SerializedMonoBehaviour
    where T : BaseGameManager<T>
{
    /// <summary>
    /// 单例
    /// </summary>
    public static T Instance
    {
        get 
        { 
            if(_instance == null)
            {
                //Debug.LogError("违规调用");
            }
            return _instance;
        }
    }
    private static T _instance;
    //管理器组
    public FactoryManager FactoryManager
    {
        get;
        private set;
    }
    public AudioManager AudioManager
    {
        get;
        private set;
    }

    public EventCenter EventCenter
    {
        get;
        private set;
    }
    public MapManager MapManager
    {
        get;
        private set;
    }
    public ControlManager ControlManager
    {
        get;
        private set;
    }
    public PanelManager PanelManager
    {
        get;
        private set;
    }
    public SceneObjectManager SceneObjectManager
    {
        get;
        private set;
    }

    /// <summary>
    /// 资源路径配置文件
    /// </summary>
    public GameConfig GameConfig
    {
        get;
        private set;
    }

    /// <summary>
    /// 当前场景
    /// </summary>
    public Config.SceneInfo CurrentScene
    {
        get;
        protected set;
    }

    /// <summary>
    /// 游戏状态
    /// </summary>
    public GameStatus Status
    {
        get;
        protected internal set;
    }

    /// <summary>
    /// 开始切换场景
    /// </summary>
    public event System.Action<MapPosition> BeginToLoadPosition;

    /// <summary>
    /// 场景初始化中
    /// </summary>
    public event System.Action<MapPosition> OnLoadingPosition;

    /// <summary>
    /// 场景切换完成
    /// </summary>
    public event System.Action<MapPosition> AfterLoadingPosition;

    protected virtual void Awake()
    {
        if(_instance != null)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this as T;
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// 初始化管理器
    /// </summary>
    /// <typeparam name="TInstaller"></typeparam>
    protected virtual void InitManager<TInstaller>() where TInstaller : IFactoryInstaller, new()
    {
        GameConfig = Resources.Load<GameConfig>("Config/全局配置");
        FactoryManager = new FactoryManager();
        AudioManager = FindObjectOfType<AudioManager>();
        ControlManager = GetComponent<ControlManager>();
        EventCenter = GetComponent<EventCenter>();
        MapManager = new MapManager();
        //PanelManager = new PanelManager();
        SceneObjectManager = GetComponent<SceneObjectManager>();

        //初始化管理器类
        TInstaller installer = new TInstaller();
        installer.Install(FactoryManager);
        if (FactoryManager.Load(out SettingSave setting, "Setting.sav"))
        {
            AudioManager.Init(setting.SoundSave);
            ControlManager.Init(setting.ControlSave);
        }
        else
        {
            AudioManager.Init(new SoundSave
            {
                Effect = 1,
                Music = 1
            });
            ControlManager.Init(null);
        }

        SceneObjectManager.Init();
    }

    /// <summary>
    /// 初始化游戏状态
    /// </summary>
    protected virtual void InitGameStatus()
    {
        Status = GameStatus.Pause;
    }

    /// <summary>
    /// <inheritdoc cref="AudioManager.Play(AudioClip, MusicPath, bool)"/>
    /// </summary>
    /// <param name="name">资源名</param>
    /// <param name="path">音频通道</param>
    /// <param name="isLoop">是否循环</param>
    public void PlaySound(string name, MusicPath path, bool isLoop = false)
    {
        AudioManager.Play((FactoryManager.Create(ObjectType.AudioClip, name) as PAudioClip).AudioClip, path, isLoop);
    }

    /// <summary>
    /// 转移到指定地点
    /// </summary>
    /// <param name="position"></param>
    public void EnterPosition(MapPosition position)
    {
        //var oldPosition = GameManager.CurrentScene;
        //GameManager.Status = GameStatus.Loading;
        ////等待渐出动画播放完毕再进行转移
        ////PanelManager.Push(WindowType.loading);
        //PanelManager.Peek().StartCoroutine(PanelManager.Peek().ObserveAnimProgress("Enter", 1, () => 
        //{
        //    Debug.Log($"正在从{oldPosition.name}跳转至{position.Scene}:{position.Point}");
        //    //如果大场景没发生变化则直接跳转
        //    if (GameConfig.SceneInfoConfig.ScenesObject[position.Scene]
        //        != GameManager.CurrentScene)
        //    {
        //        MapManager.LoadScene(GameConfig.SceneInfoConfig.
        //            ScenesObject[position.Scene].Scene, () =>
        //            {
        //                LoadPosition(position);
        //            });
        //    }
        //    else
        //    {
        //        LoadPosition(position);
        //    }
        //}));
        //BeginToLoadPosition?.Invoke(position);
    }

    /// <summary>
    /// 载入场景
    /// </summary>
    /// <remarks>
    /// 初始化地图，即生成场景物品
    /// </remarks>
    protected void LoadPosition(MapPosition position)
    {
        OnLoadingPosition?.Invoke(position);
        Status = GameStatus.Playing;
        AfterLoadingPosition?.Invoke(position);
    }
}
