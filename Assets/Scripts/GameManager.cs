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
using Newtonsoft.Json;

/// <summary>
/// 负责处理游戏运行逻辑，存储了游戏运行的相关字段
/// </summary>
/// <remarks>
/// 游戏入口点，负责游戏系统初始化
/// </remarks>
public class GameManager : IGameManager
{
    private GameSystem _gameSystem;
    public GameSystem GameSystem
    {
        get
        {
            return _gameSystem;
        }
    }

    /// <summary>
    /// 全局访问点
    /// </summary>
    public static GameManager Instance;

    [SerializeField]
    private BehaviorStateMachine _player;
    /// <summary>
    /// 当前操作的角色
    /// </summary>
    public BehaviorStateMachine Player
    {
        get
        {
            return _player;
        }
        set
        {
            _player = value;
        }
    }

    private GameSave _gameSave;
    public GameSave GameSave
    {
        get
        {
            return _gameSave;
        }
        internal set
        {
            _gameSave = value;
        }
    }

    private void Awake()
    {
        //单例模式初始化
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this);

        InitGameSystem();
        InitGameStatus();
    }

    private void InitGameStatus()
    {
        //游戏状态初始化
        Status = GameStatus.Pause;
        //CurrentScene = GameSystem.GameConfig.SceneInfoConfig.ScenesObject["主菜单界面"];
        //LoadGameSave();
        //GameSystem.EventCenter.Init(GameSave.Story.MessagePath);
    }

    private void InitGameSystem()
    {
        //依赖注入管理类
        _gameSystem = new GameSystem();
        _gameSystem.GetType().GetProperty("GameConfig")
            .SetValue(_gameSystem, Resources.Load<GameConfig>("Config/全局配置"));
        _gameSystem.GetType().GetProperty("FactoryManager")
            .SetValue(_gameSystem, new FactoryManager());
        _gameSystem.GetType().GetProperty("AudioManager")
            .SetValue(_gameSystem, FindObjectOfType<AudioManager>());
        _gameSystem.GetType().GetProperty("ControlManager")
            .SetValue(_gameSystem, GetComponent<ControlManager>());
        _gameSystem.GetType().GetProperty("EventCenter")
            .SetValue(_gameSystem, GetComponent<EventCenter>());
        _gameSystem.GetType().GetProperty("MapManager")
            .SetValue(_gameSystem, new MapManager());
        _gameSystem.GetType().GetProperty("PanelManager")
            .SetValue(_gameSystem, new PanelManager());
        _gameSystem.GetType().GetProperty("SceneObjectManager")
            .SetValue(_gameSystem, GetComponent<SceneObjectManager>());

        //初始化管理器类
        FactoryInstaller.Install(GameSystem.FactoryManager);
        if (GameSystem.FactoryManager.Load(out SettingSave setting, "Setting.sav"))
        {
            GameSystem.AudioManager.Init(setting.SoundSave);
            GameSystem.ControlManager.Init(setting.ControlSave);
        }
        else
        {
            GameSystem.AudioManager.Init(new SoundSave
            {
                Effect = 1,
                Music = 1
            });
            GameSystem.ControlManager.Init(null);
        }

        GameSystem.SceneObjectManager.Init();
        GameSystem.Init(this);

        GameSystem.OnLoadingPosition += (p) =>
        {
            CurrentScene = GameSystem.GameConfig.SceneInfoConfig.ScenesObject[p.Scene];
            LoadPlayer(p);
        };
    }

    private void Start()
    {
        //打开主界面
        //GameSystem.PanelManager.Push(WindowType.StartMenu);
    }

    /// <summary>
    /// 开始游戏
    /// </summary>
    public void GameStart()
    {
        GameSystem.PanelManager.Remove(WindowType.StartMenu);

        //重置存档
        GameSave = GameSave.OriSave();

        //加载场景
        Status = GameStatus.Loading;
        GameSystem.EnterPosition(GameSave.Position);
    }

    /// <summary>
    /// 让游戏进入暂停状态
    /// </summary>
    public void PauseGame()
    {
        Status = GameStatus.Pause;
    }

    /// <summary>
    /// 恢复游玩状态
    /// </summary>
    public void ResumeGame()
    {
        Status = GameStatus.Playing;
    }

    /// <summary>
    /// 退出游戏
    /// </summary>
    public void Quit()
    {
        Application.Quit();
    }

    /// <summary>
    /// 游戏失败
    /// </summary>
    public void GameOver()
    {

    }

    private void LoadGameSave()
    {
        string filePath = Application.persistentDataPath + "Slot1.sav";
        if (!File.Exists(filePath))
        {
            GameSave = GameSave.OriSave();
            return;
        }
        StreamReader file = new StreamReader(filePath);
        string json = file.ReadToEnd();
        Debug.Log(json);
        GameSave = JsonConvert.DeserializeObject<GameSave>(json);
    }

    private void SaveGameSave()
    {
        string filePath = Application.persistentDataPath + "Slot1.sav";
        var json = JsonConvert.SerializeObject(GameSave, Formatting.Indented);
        Debug.Log(json);
        string s = filePath.Substring(0, filePath.LastIndexOf('/'));
        Directory.CreateDirectory(s);
        StreamWriter file = new StreamWriter(filePath);
        file.Write(json);
        file.Close();
    }

    private void LoadPlayer(MapPosition position)
    {
        var p = GameSystem.FactoryManager.Create(ObjectType.NPC, "Shury") as PNPC;
        p.StateMachine.gameObject.transform.position = CurrentScene.Positions[position.Point];
        GameObject.Find("CM vcam1").GetComponent<Cinemachine.CinemachineVirtualCamera>().Follow = p.StateMachine.transform;
    }
}
