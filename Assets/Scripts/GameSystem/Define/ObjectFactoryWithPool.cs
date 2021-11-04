using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// 带对象池的对象工厂
/// </summary>
/// <typeparam name="T">对象类型</typeparam>
/// <remarks>
/// 仅进行读取简单的原始数据读取，不适用与如角色等复杂结构对象
/// </remarks>
public class ObjectFactoryWithPool
{
    private Dictionary<string, Counter> _pool;

    /// <summary>
    /// 对象池最大数量
    /// </summary>
    private int _maxNum;

    /// <summary>
    /// 对象生成委托
    /// </summary>
    /// <remarks>
    /// 当对象池内查询不到索引时，调用该委托创建对象
    /// </remarks>
    private Func<string, IProduct> CreatObject;

    /// <param name="creatObject">创建对象的方法</param>
    /// <param name="capacity">对象池容量</param>
    public ObjectFactoryWithPool(Func<string, IProduct> creatObject, int capacity = int.MaxValue)
    {
        CreatObject = creatObject;
        _maxNum = capacity;
        _pool = new Dictionary<string, Counter>(capacity);
    }

    /// <summary>
    /// 创建对象
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public IProduct Create(string name)
    {
        if (_pool.ContainsKey(name))
        {
            _pool[name].Time += 1;
            return _pool[name].Resource.Clone();
        }

        IProduct res = CreatObject.Invoke(name);
        Push(name, res);
        return res.Clone();
    }

    /// <summary>
    /// 清空对象池
    /// </summary>
    public void Clear()
    {
        _pool = new Dictionary<string, Counter>();
    }

    /// <summary>
    /// 将对象池清理至指定数目
    /// </summary>
    /// <remarks>
    /// 可以通过重写，改变清理策略
    /// </remarks>
    protected virtual void ResetPool(int capacity)
    {

        if (capacity >= _pool.Count)
        {
            return;
        }
        if (capacity == 0)
        {
            Clear();
            return;
        }

        //TODO: 优化删除项筛选
        int toDelete = _pool.Count - capacity;
        var sorters = new List<Sorter>(toDelete);
        int num = 0;

        string[] keys = new string[_pool.Count];
        _pool.Keys.CopyTo(keys, 0);
        //删除使用最少的且最早加入的对象
        for (int i = _pool.Count - 1; i >= 0; --i)
        {
            if (num < toDelete)
            {
                sorters[num++] = new Sorter
                {
                    Key = keys[i],
                    Time = _pool[keys[i]].Time
                };
            }
            else
            {
                //替换掉引用数最大的数据（先试试看效果）
                sorters.Sort((Sorter a, Sorter b) => {
                    if (a.Time > b.Time) return 1;
                    else if (a.Time == b.Time) return 0;
                    else return -1;
                });
                if (sorters[0].Time >= _pool[keys[i]].Time)
                {
                    sorters[0] = new Sorter
                    {
                        Key = keys[i],
                        Time = _pool[keys[i]].Time
                    };
                }
            }

            //防止数据沉积，让每一次遍历都减少一次引用数量
            if (_pool[keys[i]].Time > 0)
            {
                _pool[keys[i]].Time -= 1;
            }
        }

        //清除对应键值对
        foreach (var p in sorters)
        {
            _pool.Remove(p.Key);
        }
    }

    /// <summary>
    /// 将指定对象推入池中
    /// </summary>
    /// <param name="name"></param>
    public virtual void Push(string name, IProduct obj)
    {
        if (_pool.Count + 1 > _maxNum)
        {
            ResetPool(_maxNum - 1);
        }
        _pool.Add(name, new Counter { Resource = obj, Time = 1 });
    }

    /// <summary>
    /// 将指定对象从池中取出
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    /// <remarks>
    /// 如果希望创建的对象不存储于对象池，请调用该方法
    /// </remarks>
    public virtual IProduct Pull(string name)
    {
        if (_pool.ContainsKey(name))
        {
            var temp = _pool[name].Resource;
            _pool.Remove(name);
            return temp;
        }
        return CreatObject.Invoke(name);
    }

    /// <summary>
    /// 计数器
    /// </summary>
    private class Counter
    {
        public IProduct Resource;
        public int Time;
    }

    /// <summary>
    /// 用来给资源排序
    /// </summary>
    private struct Sorter
    {
        public int Time;
        public string Key;
    }
}
