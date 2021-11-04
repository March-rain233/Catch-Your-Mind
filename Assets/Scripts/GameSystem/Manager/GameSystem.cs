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
/// ��������࣬��Ϊ�н��߰Ѳ����ַ�������������
/// </summary>
/// <remarks>
/// �������Ĺ��ܿ������������벻Ҫ��������������
/// </remarks>
[System.Serializable]
public class GameSystem
{
    public static GameSystem Instance
    {
        get
        {
            if(_instance == null)
            {
                Debug.LogError("Υ�����GameSystem");
            }
            return _instance;
        }
    }
    private static GameSystem _instance;

    //��������
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
    /// ��Ϸ������
    /// </summary>
    public IGameManager GameManager
    {
        get;
        private set;
    }

    /// <summary>
    /// ��Դ·�������ļ�
    /// </summary>
    public GameConfig GameConfig
    {
        get;
        private set;
    }

    /// <summary>
    /// ��ʼ�л�����
    /// </summary>
    public event System.Action<MapPosition> BeginToLoadPosition;

    /// <summary>
    /// ������ʼ����
    /// </summary>
    public event System.Action<MapPosition> OnLoadingPosition;

    /// <summary>
    /// �����л����
    /// </summary>
    public event System.Action<MapPosition> AfterLoadingPosition;

    public GameSystem()
    {
        if(_instance != null)
        {
            Debug.LogError("Υ������GameSystem");
        }
        _instance = this;
    }

    /// <summary>
    /// <inheritdoc cref="AudioManager.Play(AudioClip, MusicPath, bool)"/>
    /// </summary>
    /// <param name="name">��Դ��</param>
    /// <param name="path">��Ƶͨ��</param>
    /// <param name="isLoop">�Ƿ�ѭ��</param>
    public void PlaySound(string name, MusicPath path, bool isLoop = false)
    {
        AudioManager.Play((FactoryManager.Create(ObjectType.AudioClip, name) as PAudioClip).AudioClip, path, isLoop);
    }

    /// <summary>
    /// ת�Ƶ�ָ���ص�
    /// </summary>
    /// <param name="position"></param>
    public void EnterPosition(MapPosition position)
    {
        var oldPosition = GameManager.CurrentScene;
        GameManager.Status = GameStatus.Loading;
        //�ȴ�����������������ٽ���ת��
        PanelManager.Push(WindowType.loading);
        PanelManager.Peek().StartCoroutine(PanelManager.Peek().ObserveAnimProgress("Enter", 1, () => 
        {
            Debug.Log($"���ڴ�{oldPosition.name}��ת��{position.Scene}:{position.Point}");
            //����󳡾�û�����仯��ֱ����ת
            if (GameConfig.SceneInfoConfig.ScenesObject[position.Scene]
                != GameManager.CurrentScene)
            {
                MapManager.LoadScene(GameConfig.SceneInfoConfig.
                    ScenesObject[position.Scene].Scene, () =>
                    {
                        LoadPosition(position);
                    });
            }
            else
            {
                LoadPosition(position);
            }
        }));
        BeginToLoadPosition?.Invoke(position);
    }

    /// <summary>
    /// ���볡��
    /// </summary>
    /// <remarks>
    /// ��ʼ����ͼ�������ɳ�����Ʒ
    /// </remarks>
    private void LoadPosition(MapPosition position)
    {
        OnLoadingPosition?.Invoke(position);
        GameManager.Status = GameStatus.Playing;
        AfterLoadingPosition?.Invoke(position);
    }

    public void Init(IGameManager manager)
    {
        GameManager = manager;
        AfterLoadingPosition += (p) =>
        {
            PlaySound(GameConfig.SceneInfoConfig.ScenesObject[p.Scene].BGM,
                MusicPath.Music, true);
        };
        AfterLoadingPosition += (p) =>
        {
            PanelManager.Remove(WindowType.loading);
        };
    }
}
