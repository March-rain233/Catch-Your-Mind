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
public class GameManager : BaseGameManager<GameManager>
{

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

    public EventTree.EventTree EventTree;

    /// <summary>
    /// 剩余时间
    /// </summary>
    public float RemainTime
    {
        get => _remainTime;
        set
        {
            _remainTime = value;
            TimeChanged?.Invoke(_remainTime);
        }
    }
    private float _remainTime;

    /// <summary>
    /// 最大时间
    /// </summary>
    public float MaxTime;

    /// <summary>
    /// 每秒流逝时间
    /// </summary>
    public float DeltaTime;

    /// <summary>
    /// 时间是否在流逝
    /// </summary>
    public bool Passing;

    /// <summary>
    /// 时间变化
    /// </summary>
    public System.Action<float> TimeChanged;

    protected override void Awake()
    {
        base.Awake();
        InitManager<FactoryInstaller>();
        EventTree.Init();

        LoadGameSave();

        InitGameStatus();
    }

    protected override void InitGameStatus()
    {
        base.InitGameStatus();
        Passing = false;
        RemainTime = MaxTime;
    }

    private void Update()
    {
        if (Passing && RemainTime > 0)
        {
            RemainTime -= DeltaTime * Time.deltaTime;
        }
    }

    /// <summary>
    /// 开始游戏
    /// </summary>
    public void GameStart()
    {
        //GameManager.PanelManager.Remove(WindowType.StartMenu);

        //重置存档
        GameSave = GameSave.OriSave();

        //加载场景
        Status = GameStatus.Loading;
        //GameManager.EnterPosition(GameSave.Position);
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
        var p = GameManager.Instance.FactoryManager.Create(ObjectType.NPC, "Shury") as PNPC;
        p.StateMachine.gameObject.transform.position = CurrentScene.Positions[position.Point];
        GameObject.Find("CM vcam1").GetComponent<Cinemachine.CinemachineVirtualCamera>().Follow = p.StateMachine.transform;
    }

    [Button("发送事件（仅供测试）")]
    private void SendEvent(string name, EventCenter.EventArgs eventArgs)
    {
        EventCenter.SendEvent(name, eventArgs);
    }
}
