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

    protected override void Awake()
    {
        base.Awake();
        InitManager<FactoryInstaller>();
        InitGameStatus();
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
}
