using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 需要通过工厂生成的都需要继承这个接口
/// </summary>
public interface IProduct
{
    IProduct Clone();
}

/// <summary>
/// 精灵包装器
/// </summary>
public class PSprite : IProduct
{
    public Sprite Sprite;
    public IProduct Clone()
    {
        return this;
    }
}

/// <summary>
/// 音效片段包装器
/// </summary>
public class PAudioClip : IProduct
{
    public AudioClip AudioClip;
    public IProduct Clone()
    {
        return this;
    }
}