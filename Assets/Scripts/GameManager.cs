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
using UnityEngine.UI;
using DG.Tweening;

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
            if(_remainTime < 0)
            {
                GameOver();
            }
        }
    }
    [SerializeField]
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

        SceneManager.activeSceneChanged += SceneManager_activeSceneChanged;
        EventCenter.AddListener("DIALOG_PUSH", (e) => { Passing = false; });
        EventCenter.AddListener("DIALOG_EXIT", (e) => { if(SceneManager.GetActiveScene().name!="MakerScene") Passing = true; });
    }

    private void SceneManager_activeSceneChanged(Scene arg0, Scene arg1)
    {
        if (arg1.name != "MainMenu")
        {
            GameSave.SceneIndex = arg1.buildIndex;
            SaveGameSave();
        }
        if (arg1.name == "MakerScene") { Passing = false; }
        if(arg1.name == "Home")
        {
            float timeCount = 0;
            DOTween.To(() => timeCount, a => timeCount = a, 1, 0.3f).OnComplete(()=>EventCenter.SendEvent("InteractionEnter", new EventCenter.EventArgs() { String = "按E进行对话" }));
        }
        else { Passing = true; }
        if(arg1.name == "OutSide")
        {
            if(EventCenter.TryGetEventArgs("进入MakerScene", out EventCenter.EventArgs eventArgs) && eventArgs.Boolean)
            {
                GameObject.Find("Mind").transform.position = new Vector3(-16, -3.1f, 0);
                GameObject.Find("EMO").transform.position = new Vector3(-17, -1f, 0);
            }
        }
        Instance.EventCenter.SendEvent($"进入{arg1.name}", new EventCenter.EventArgs() { Boolean = true });
        var mask = GameObject.Find("SceneMask").GetComponent<CanvasGroup>();
        mask.blocksRaycasts = true;
        mask.alpha = 1;
        mask.DOFade(0, 0.5f).SetDelay(0.3f).onComplete = () =>
        {
            mask.blocksRaycasts = false;
            EventCenter.SendEvent("DIALOG_EXIT", new EventCenter.EventArgs() { Boolean = true }); 
        };
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
        GameSave.Time = MaxTime;
        Passing = true;

        //加载场景
        //Status = GameStatus.Loading;
        //GameManager.EnterPosition(GameSave.Position);
        SceneManager.LoadScene("Home");
    }

    public void LoadGame()
    {
        SceneManager.LoadScene(GameSave.SceneIndex);
    }

    [Button]
    public void LoadScene(string name)
    {
        EventCenter.SendEvent("DIALOG_ENTER", new EventCenter.EventArgs() { Boolean = true });
        var mask = GameObject.Find("SceneMask").GetComponent<CanvasGroup>();
        mask.blocksRaycasts = true;
        mask.DOFade(1, 0.5f).onComplete = () => { SceneManager.LoadScene(name); };
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
        UnityEngine.SceneManagement.SceneManager.LoadScene("Failure");
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
        file.Close();
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
