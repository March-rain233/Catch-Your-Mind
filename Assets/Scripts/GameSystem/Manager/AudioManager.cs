using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;

/// <summary>
/// 负责音效的管理
/// </summary>
public class AudioManager : SerializedMonoBehaviour
{

    /// <summary>
    /// 音量配置
    /// </summary>
    private Save.SoundSave _save;

    /// <summary>
    /// 声效播放器列表
    /// </summary>
    public List<AudioSource> SoundEffect;

    /// <summary>
    /// 音乐播放器列表
    /// </summary>
    public List<AudioSource> Music;

    /// <summary>
    /// 音效大小
    /// </summary>
    [SerializeField, SetProperty("SoundEffectVolume")]
    private float _soundEffectVolume = 1;
    public float SoundEffectVolume 
    {
        get
        {
            return _soundEffectVolume;
        }
        set
        {
            if (value < 0) value = 0;
            if (value > 1) value = 1;
            _soundEffectVolume = value;
            foreach (var audio in SoundEffect)
            {
                audio.volume = _soundEffectVolume;
            }
        }
    }

    /// <summary>
    /// 音乐大小
    /// </summary>
    [SerializeField, SetProperty("MusicVolume")]
    private float _musicVolume = 1;
    public float MusicVolume
    {
        get
        {
            return _musicVolume;
        }
        set
        {
            if (value < 0) value = 0;
            if (value > 1) value = 1;
            _musicVolume = value;
            foreach (var audio in Music)
            {
                audio.volume = _musicVolume;
            }
        }
    }

    /// <summary>
    /// 外界调用来初始化
    /// </summary>
    /// <param name="save"></param>
    public void Init(Save.SoundSave save)
    {
        _save = save;
        MusicVolume = _save.Music;
        SoundEffectVolume = _save.Effect;
    }

    /// <summary>
    /// 播放
    /// </summary>
    /// <param name="clip">音频资源</param>
    /// <param name="isLoop">是否循环播放</param>
    /// <param name="path">音乐通道</param>
    /// <param name="cover">当相同的音效正在播放时是否覆盖他（如果是音乐则重新播放，是音效则再创建声道）</param>
    public void Play(AudioClip clip, MusicPath path, bool isLoop = false, bool cover = false)
    {
        if(!cover && IsPlaying(clip, path))
        {
            return;
        }
        if(cover && path == MusicPath.Music)
        {
            CancelPlay(clip, path);
        }

        List<AudioSource> cols;
        float volume;
        switch (path)
        {
            case MusicPath.Music:
                cols = Music;
                volume = MusicVolume;
                break;
            case MusicPath.SoundEffect:
                cols = SoundEffect;
                volume = SoundEffectVolume;
                break;
            default:
                cols = new List<AudioSource>();
                volume = 0;
                break;
        }

        //查询是否有空闲的播放器，若存在则使用该播放器播放
        foreach (var sound in cols)
        {
            if (!sound.isPlaying)
            {
                sound.loop = isLoop;
                sound.clip = clip;
                sound.volume = volume;
                sound.Play();
                return;
            }
        }

        //当不存在播放器时新建播放器
        var audio = gameObject.AddComponent<AudioSource>();
        audio.clip = clip;
        audio.loop = isLoop;
        audio.volume = volume;
        audio.Play();
        cols.Add(audio);
        return;
    }

    /// <summary>
    /// 停止指定通道的所有资源播放
    /// </summary>
    /// <param name="path">音效通道</param>
    public void Clear(MusicPath path)
    {
        List<AudioSource> t;
        switch (path)
        {
            case MusicPath.Music:
                t = Music;
                break;
            case MusicPath.SoundEffect:
                t = SoundEffect;
                break;
            default:
                t = new List<AudioSource>();
                break;
        }
        foreach(var temp in t)
        {
            temp.Stop();
        }
    }

    /// <summary>
    /// 判断指定资源是否在播放
    /// </summary>
    /// <param name="clip">音效资源</param>
    /// <returns></returns>
    public bool IsPlaying(AudioClip clip, MusicPath path)
    {
        List<AudioSource> cols;
        switch (path)
        {
            case MusicPath.Music:
                cols = Music;
                break;
            case MusicPath.SoundEffect:
                cols = SoundEffect;
                break;
            default:
                cols = new List<AudioSource>();
                break;
        }

        //遍历通道查看是否有对应片段在播放
        foreach (var i in cols)
        {
            if(i.clip == clip && i.isPlaying)
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// 关闭指定资源的播放
    /// </summary>
    /// <param name="clip">音效资源</param>
    public void CancelPlay(AudioClip clip, MusicPath path)
    {
        float volume;
        List<AudioSource> cols;
        switch (path)
        {
            case MusicPath.Music:
                cols = Music;
                volume = MusicVolume;
                break;
            case MusicPath.SoundEffect:
                cols = SoundEffect;
                volume = SoundEffectVolume;
                break;
            default:
                cols = new List<AudioSource>();
                volume = 0;
                break;
        }

        //遍历播放器并停止指定资源的播放
        foreach (var sound in cols)
        {
            if (sound.isPlaying && sound.clip == clip)
            {
                sound.Stop();
                sound.clip = null;
            }
        }
    }

    /// <summary>
    /// 设置音效大小
    /// </summary>
    /// <param name="Volume">音效大小[0,1]</param>
    public void SetSoundEffectVolume(float Volume)
    {
        SoundEffectVolume = Volume;
        _save.Effect = ((int)(_soundEffectVolume * Mathf.Pow(10, 5))) / Mathf.Pow(10, 5);
        GameManager.Instance.FactoryManager.Save(_save, "/Setting/setting.sav");
    }

    /// <summary>
    /// 设置音乐大小
    /// </summary>
    /// <param name="Volume">音乐大小[0,1]</param>
    public void SetMusicVolume(float Volume)
    {
        MusicVolume = Volume;
        _save.Music = ((int)(_musicVolume * Mathf.Pow(10,5))) / Mathf.Pow(10, 5);
        GameManager.Instance.FactoryManager.Save(_save, "/Setting/setting.sav");
    }
}
