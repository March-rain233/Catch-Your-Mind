using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using Sirenix.OdinInspector;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using UnityEngine.SceneManagement;

/// <summary>
    /// 音乐声道
    /// </summary>
public enum MusicPath
    {
        Music,
        SoundEffect,
    }
/// <summary>
    /// 游戏运行状态
    /// </summary>
public enum GameStatus
    {
        /// <summary>
        /// 玩家控制状态
        /// </summary>
        Playing,
        /// <summary>
        /// 游戏暂停状态，如进入UI界面，以及菜单等
        /// </summary>
        Pause,
        /// <summary>
        /// 载入状态
        /// </summary>
        Loading,
    }
/// <summary>
/// 自定义工具
/// </summary>
namespace MyTools
{

    /// <summary>
    /// 后缀表达式转换器
    /// </summary>
    public class Postfix
    {
        /// <summary>
        /// 左括号
        /// </summary>
        string left = "\\(";

        /// <summary>
        /// 右括号
        /// </summary>
        string right = "\\)";

        /// <summary>
        /// 符号-优先度 字典
        /// </summary>
        Dictionary<string, int> priority = new Dictionary<string, int> { { "\\(", 100 }, { "\\)", 100 } }; //越小优先级越高

        /// <summary>
        /// 运算域
        /// </summary>
        List<string> fld = new List<string>();

        public Postfix()
        {

        }

        public Postfix(Postfix old)
        {
            left = old.left;
            right = old.right;
            priority = new Dictionary<string, int>(old.priority);
            fld = new List<string>(old.fld);
        }

        /// <summary>
        /// 将算式转换成后缀表达式
        /// </summary>
        /// <param name="resource">
        /// 算式
        /// </param>
        /// <returns>
        /// 后缀表达式
        /// </returns>
        public List<string> Translate(string resource)
        {
            Stack<string> sign = new Stack<string>();
            List<string> result = new List<string>();

            //将导入表达式拆解
            while (resource.Length != 0)
            {
                //所查找出的匹配 运算符|值
                string compare;

                //判断是否为值，是则直接压入表达式
                if (IsNumber(resource, out compare))
                {
                    result.Add(compare);
                    resource = Regex.Replace(resource, $"^{compare}", "");
                }

                //判断是否为运算符
                else if (IsSign(resource, out compare))
                {
                    resource = Regex.Replace(resource, $"^{compare}", "");

                    //括号处理
                    if (compare == left)
                    {
                        //将左括号压入符号栈
                        sign.Push(compare);
                    }
                    else if (compare == right)
                    {
                        //将符号栈中直至左括号的所有运算符导入表达式中
                        while (sign.Peek() != left)
                        {
                            result.Add(sign.Pop());
                        }
                        sign.Pop();
                    }

                    //常规符号处理
                    //当栈内无符号时直接压入
                    else if (sign.Count == 0)
                    {
                        sign.Push(compare);
                    }

                    //当当前符号优先级小于栈顶符号优先级时，将当前符号压入符号栈
                    else if (priority[compare] < priority[sign.Peek()])
                    {
                        sign.Push(compare);
                    }

                    //当当前符号优先级大于栈顶符号优先级时，
                    //将所有小于当前符号优先级的符号导入表达式，并将当前符号压入符号栈
                    else if (priority[compare] >= priority[sign.Peek()])
                    {
                        while (sign.Count != 0 && priority[compare] >= priority[sign.Peek()])
                        {
                            result.Add(sign.Pop());
                        }
                        sign.Push(compare);
                    }
                }
            }

            //将所有符号从栈内取出并压入表达式
            while (sign.Count != 0)
            {
                result.Add(sign.Pop());
            }
            return result;
        }

        /// <summary>
        /// 添加运算符
        /// </summary>
        /// <param name="sign">
        /// 运算符对应字符串
        /// </param>
        /// <param name="i">
        /// 运算符优先级
        /// </param>
        public void AddPriority(string sign, int i)
        {
            if (priority.ContainsKey(sign)) priority[sign] = i;
            else priority.Add(sign, i);
        }

        /// <summary>
        /// 添加运算域
        /// </summary>
        /// <param name="value">
        /// 变量名
        /// </param>
        public void AddFld(string value)
        {
            fld.Add(value);
        }

        /// <summary>
        /// 判断是否是变量
        /// </summary>
        /// <param name="target">
        /// 待判断字符串
        /// </param>
        /// <param name="compare">
        /// 对应字符串
        /// </param>
        /// <returns></returns>
        private bool IsNumber(string target, out string compare)
        {
            foreach (var i in fld)
            {
                if (Regex.IsMatch(target, $"^{i}"))
                {
                    compare = i;
                    return true;
                }
            }
            compare = "";
            return false;
        }

        /// <summary>
        /// 判断是否是运算符
        /// </summary>
        /// <param name="target">
        /// 待判断字符串
        /// </param>
        /// <param name="compare">
        /// 对应字符串
        /// </param>
        /// <returns></returns>
        private bool IsSign(string target, out string compare)
        {
            foreach (var i in priority.Keys)
            {
                if (Regex.IsMatch(target, $"^{i}"))
                {
                    compare = i;
                    return true;
                }
            }
            compare = "";
            return false;
        }
    }

    /// <summary>
    /// 计算器类
    /// </summary>
    /// <typeparam name="T">运算域实例类型</typeparam>
    public class Calculator<T>
    {
        /// <summary>
        /// 当前使用的后缀表达式转换器
        /// </summary>
        private Postfix postfix = new Postfix();

        /// <summary>
        /// 双目运算符集
        /// </summary>
        Dictionary<string, Func<T, T, T>> doubleSign = new Dictionary<string, Func<T, T, T>>();

        /// <summary>
        /// 单目运算符集
        /// </summary>
        Dictionary<string, Func<T, T>> singleSign = new Dictionary<string, Func<T, T>>();

        /// <summary>
        /// 运算域
        /// </summary>
        Dictionary<string, T> fld = new Dictionary<string, T>();

        /// <summary>
        /// 计算输入的算式
        /// </summary>
        /// <param name="resource">
        /// 算式
        /// </param>
        /// <returns>
        /// 运算结果
        /// </returns>
        public T Calculate(string resource)
        {
            if (doubleSign.Count == 0)
            {
                return default;
            }

            var back = postfix.Translate(resource);
            Stack<T> num = new Stack<T>();

            //逐一运算后缀表达式
            foreach (var i in back)
            {
                //将值压入值栈
                if (IsValue(i))
                {
                    T temp = fld[i];
                    num.Push(temp);
                }

                //从值栈中提取一个元素进行对应单目运算并将结果压入值栈
                else if (IsSingleSign(i))
                {
                    T num1;
                    num1 = num.Pop();
                    num.Push(singleSign[i](num1));
                }

                //从值栈中提取两个元素进行对应双目运算并将结果压入值栈
                else if (IsDoubleSign(i))
                {
                    T num1, num2;
                    num2 = num.Pop();
                    num1 = num.Pop();
                    num.Push(doubleSign[i](num1, num2));
                }
            }
            return num.Peek();
        }

        /// <summary>
        /// 添加双目运算符
        /// </summary>
        /// <param name="name">
        /// 运算符对应字符串
        /// </param>
        /// <param name="func">
        /// 运算符函数
        /// </param>
        /// <param name="i">
        /// 运算符优先级
        /// </param>
        public void AddFunc(string name, Func<T, T, T> func, int i)
        {
            doubleSign[name] = func;
            postfix.AddPriority(name, i);
        }

        /// <summary>
        /// 添加单目运算符
        /// </summary>
        /// <param name="name">
        /// 运算符对应字符串
        /// </param>
        /// <param name="func">
        /// 运算符函数
        /// </param>
        /// <param name="i">
        /// 运算符优先级
        /// </param>
        public void AddFunc(string name, Func<T, T> func, int i)
        {
            singleSign[name] = func;
            postfix.AddPriority(name, i);
        }

        /// <summary>
        /// 判断是否为运算符
        /// </summary>
        /// <param name="name">待判断字符串</param>
        /// <returns></returns>
        private bool IsFunc(string name)
        {
            return (doubleSign.ContainsKey(name) || singleSign.ContainsKey(name));
        }

        /// <summary>
        /// 添加运算域
        /// </summary>
        /// <param name="name">
        /// 变量对应的名字
        /// </param>
        /// <param name="value">
        /// 变量的值
        /// </param>
        public void AddFld(string name, T value)
        {
            fld[name] = value;
            postfix.AddFld(name);
        }

        /// <summary>
        /// 获取后缀表达式转换器
        /// </summary>
        /// <returns>
        /// 当前的后缀表达式转换器
        /// </returns>
        public Postfix GetPostfix()
        {
            return postfix;
        }

        /// <summary>
        /// 判断是否是变量
        /// </summary>
        /// <param name="target">
        /// 名字
        /// </param>
        /// <returns></returns>
        bool IsValue(string target)
        {
            return fld.ContainsKey(target);
        }

        /// <summary>
        /// 判断是否是双目运算符
        /// </summary>
        /// <param name="target">
        /// 名字
        /// </param>
        /// <returns></returns>
        bool IsDoubleSign(string target)
        {
            return doubleSign.ContainsKey(target);
        }

        /// <summary>
        /// 判断是否是单目运算符
        /// </summary>
        /// <param name="target">
        /// 名字
        /// </param>
        /// <returns></returns>
        bool IsSingleSign(string target)
        {
            return singleSign.ContainsKey(target);
        }
    }

    /// <summary>
    /// 自定义转换
    /// </summary>
    public static class MyConvert
    {
        /// <summary>
        /// 角度转方向向量
        /// </summary>
        /// <param name="angle">角度</param>
        /// <returns>对应方向向量</returns>
        public static Vector2 DegToVector(float angle)
        {
            angle = angle * Mathf.PI / 180;
            float x = Mathf.Cos(angle);
            float y = Mathf.Sin(angle);
            return new Vector2(x, y).normalized;
        }

        /// <summary>
        /// 方向向量转角度
        /// </summary>
        /// <param name="dir">方向向量</param>
        /// <returns>对应角度</returns>
        public static float VectorToDeg(Vector2 dir)
        {
            return Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        }

        /// <summary>
        /// 方向枚举转角度
        /// </summary>
        /// <param name="direction">方向</param>
        /// <returns>对应角度</returns>
        public static float DireToDeg(NPC.Direction direction)
        {
            switch (direction)
            {
                case NPC.Direction.Right:
                    return 45 * 0;
                case NPC.Direction.Right_Up:
                    return 45 * 1;
                case NPC.Direction.Up:
                    return 45 * 2;
                case NPC.Direction.Left_Up:
                    return 45 * 3;
                case NPC.Direction.Left:
                    return 45 * 4;
                case NPC.Direction.Left_Down:
                    return 45 * 5;
                case NPC.Direction.Down:
                    return 45 * 6;
                case NPC.Direction.Right_Down:
                    return 45 * 7;
                default:
                    return 0;
            }
        }

        /// <summary>
        /// 向量转方向枚举
        /// </summary>
        /// <param name="dir">待转换向量</param>
        /// <returns>对应方向枚举</returns>
        public static NPC.Direction VetorToDire(Vector2 dir)
        {
            float deg = VectorToDeg(dir);
            if ((deg >= 0 && deg < 22.5) || (deg >= 315 + 22.5 && deg <= 360))
            {
                return NPC.Direction.Right;
            }
            else if (deg >= 22.5 && deg < 45 + 22.5)
            {
                return NPC.Direction.Right_Up;
            }
            else if (deg >= 45 + 22.5 && deg < 90 + 22.5)
            {
                return NPC.Direction.Up;
            }
            else if (deg >= 90 + 22.5 && deg < 135 + 22.5)
            {
                return NPC.Direction.Left_Up;
            }
            else if (deg >= 135 + 22.5 && deg < 180 + 22.5)
            {
                return NPC.Direction.Left;
            }
            else if (deg >= 180 + 22.5 && deg < 225 + 22.5)
            {
                return NPC.Direction.Left_Down;
            }
            else if (deg >= 225 + 22.5 && deg < 270 + 22.5)
            {
                return NPC.Direction.Down;
            }
            else if (deg >= 270 + 22.5 && deg < 315 + 22.5)
            {
                return NPC.Direction.Right_Down;
            }
            else
            {
                return NPC.Direction.Default;
            }
        }

        /// <summary>
        /// 方向枚举转方向向量
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        public static Vector2 DireToVect(NPC.Direction dir)
        {
            return DegToVector(DireToDeg(dir));
        }
    }

    /// <summary>
    /// 常用工具
    /// </summary>
    public static class CommonTool
    {
        /// <summary>
        /// 获取枚举类型的最大值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static int GetEnumMax<T>() where T : Enum
        {
            Type t = typeof(T);
            int max = 0;
            foreach (var item in Enum.GetValues(t))
            {
                if (max < (int)item)
                {
                    max = (int)item;
                }
            }
            return max;
        }

        /// <summary>
        /// 获取枚举类型的大小
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static int GetEnumLen<T>()
        {
            Type t = typeof(T);
            Dictionary<int, string> dic = new Dictionary<int, string>();
            foreach (var item in Enum.GetValues(t))
            {
                dic.Add((int)item, Enum.GetName(t, item));
            }
            return dic.Count;
        }
    }

    /// <summary>
    /// 逻辑判断工具
    /// </summary>
    public static class ConditionTool
    {
        /// <summary>
        /// 判断表达式的真值
        /// </summary>
        /// <param name="source">表达式</param>
        /// <param name="GetBool">变量获取函数</param>
        /// <returns>判断结果</returns>
        public static bool JudgeCondition(string source, Func<string, bool> GetBool)
        {
            //当字符串为空时默认表达式为真
            bool judge = true;
            if (string.IsNullOrEmpty(source))
            {
                return judge;
            }

            //初始化真值计算器
            Calculator<bool> calculator = new Calculator<bool>();
            calculator.AddFunc("&&", And, 3);
            calculator.AddFunc("\\|\\|", Or, 2);
            calculator.AddFunc("!", Opposite, 1);
            calculator.AddFunc("\\^", Xor, 2);

            //获取并拆分表达式
            var col = Regex.Split(source, @"(?:&&)|(?:\|\|)|(?:\^)");
            for (int i = 0; i < col.Length; ++i)
            {
                col[i] = Regex.Replace(col[i], @"(!)|(\()|(\))", "");
                //添加变量及其真值
                calculator.AddFld(col[i], GetBool(col[i]));
            }
            judge = calculator.Calculate(source);

            return judge;
        }

        /// <summary>
        /// 移除字符串两端包裹的括号
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string RemoveBorder(string source)
        {
            if (source == "") return "";
            source = source.Remove(0, 1);
            source = source.Remove(source.Length - 1, 1);
            return source;
        }

        /// <summary>
        /// 或运算符
        /// </summary>
        private static bool Or(bool a, bool b)
        {
            return a | b;
        }

        /// <summary>
        /// 且运算符
        /// </summary>
        private static bool And(bool a, bool b)
        {
            return a & b;
        }

        /// <summary>
        /// 异或运算符
        /// </summary>
        private static bool Xor(bool a, bool b)
        {
            return a ^ b;
        }

        /// <summary>
        /// 取反运算符
        /// </summary>
        private static bool Opposite(bool a)
        {
            return !a;
        }
    }

    /// <summary>
    /// 范围对象检测方法集合
    /// </summary>
    public static class ObjectCheck
    {
        /// <summary>
        /// 获取扇形区域内的所有特定图层碰撞体
        /// </summary>
        /// <param name="oriPosition">初始点（圆心）</param>
        /// <param name="dir">方向</param>
        /// <param name="dis">半径</param>
        /// <param name="layer">图层</param>
        /// <param name="range">角度范围</param>
        /// <param name="accurate">精度，即射线数量</param>
        /// <returns>碰撞体数组</returns>
        public static RaycastHit2D[] FancastHit2D(Vector2 oriPosition, NPC.Direction dir, float dis = Mathf.Infinity, int layer = Physics2D.AllLayers, float range = 90f, int accurate = 15)
        {
            float angle = MyConvert.DireToDeg(dir);
            return FanShapeCheck(oriPosition, angle, dis, layer, range, accurate);
        }

        /// <summary>
        /// 获取扇形区域内的所有特定图层碰撞体
        /// </summary>
        /// <param name="oriPosition">初始点（圆心）</param>
        /// <param name="middleAngle">扇形中轴线角度</param>
        /// <param name="dis">半径</param>
        /// <param name="layer">图层</param>
        /// <param name="range">角度范围</param>
        /// <param name="accurate">精度，即射线数量</param>
        /// <returns>碰撞体数组</returns>
        public static RaycastHit2D[] FanShapeCheck(Vector2 oriPosition, float middleAngle, float dis = Mathf.Infinity, int layer = Physics2D.AllLayers, float range = 90f, int accurate = 15)
        {
            //获取最小单位格角度
            float unit = range / (accurate - 1);
            float max = middleAngle + range / 2;
            float min = middleAngle - range / 2;
            Physics2D.queriesStartInColliders = false;
            List<RaycastHit2D> result = new List<RaycastHit2D>();
            for (int i = 0; i != accurate; i++)
            {
                //获取射线上的碰撞体
                RaycastHit2D[] temp;
                Vector2 dir = MyConvert.DegToVector(Mathf.Clamp(middleAngle, min, max));
                temp = Physics2D.RaycastAll(oriPosition, dir, dis, layer);

                //将不存在于result里的碰撞体压入result
                if (temp.Length >= 0)
                {
                    foreach (var t in temp)
                    {
                        if (!result.Contains(t))
                        {
                            result.Add(t);
                        }
                    }
                    Debug.DrawRay(oriPosition, dir * dis, Color.red);
                }
                else
                {
                    Debug.DrawRay(oriPosition, dir * dis, Color.green);
                }
            }
            return result.ToArray();
        }
    }

    /// <summary>
    /// 字典序列化修饰
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    [Serializable]
    public class Serialization<TKey, TValue> : ISerializationCallbackReceiver
    {
        [SerializeField]
        List<TKey> keys;
        [SerializeField]
        List<TValue> values;

        Dictionary<TKey, TValue> target;
        public Dictionary<TKey, TValue> ToDictionary() { return target; }

        public Serialization(Dictionary<TKey, TValue> target)
        {
            this.target = target;
            OnBeforeSerialize();
        }

        public void OnBeforeSerialize()
        {
            keys = new List<TKey>(target.Keys);
            values = new List<TValue>(target.Values);
        }

        public void OnAfterDeserialize()
        {
            var count = Math.Min(keys.Count, values.Count);
            target = new Dictionary<TKey, TValue>(count);
            for (var i = 0; i < count; ++i)
            {
                target.Add(keys[i], values[i]);
            }
        }
    }
}
/// <summary>
/// 角色相关信息
/// </summary>
namespace NPC
{
    /// <summary>
    /// 角色面朝方向
    /// </summary>
    public enum Direction
    {
        Up,
        Left_Up,
        Right_Up,
        Down,
        Left_Down,
        Right_Down,
        Left,
        Right,
        Default,
    }

}
/// <summary>
/// 地图相关信息
/// </summary>
namespace Map
{
    /// <summary>
    /// 位置信息
    /// </summary>
    [Serializable]
    public struct MapPosition
    {
        /// <summary>
        /// 所在的坐标点
        /// </summary>
        public string Point;

        /// <summary>
        /// 所在的场景名
        /// </summary>
        public string Scene;
    }

    /// <summary>
    /// 地图消息，用于传递地图组件状态以及游戏进度状态
    /// </summary>
    /// <remarks>
    /// KeepTrue:持续保存的状态 
    /// <para>
    /// TempTrue:加载地图后就制空的状态
    /// </para>
    /// </remarks>
    [Flags, Serializable]
    [EnumPaging]
    public enum StoryEvent
    {
        Null = 0,
        Disable = 1,
        KeepTrue = 1 << 1,
        TempTrue = 1 << 2,
    }
}
/// <summary>
/// 存档相关
/// </summary>
namespace Save
{

    /// <summary>
    /// 场景物体生成参数
    /// </summary>
    [Serializable]
    public struct ObjectInfo
    {
        /// <summary>
        /// 生成位置
        /// </summary>
        public Vector2 Position;

        /// <summary>
        /// 生成条件
        /// </summary>
        public BaseAssertion CreatReason;

        /// <summary>
        /// 销毁条件
        /// </summary>
        public BaseAssertion DestoryReason;

        /// <summary>
        /// 对象启用条件
        /// </summary>
        public BaseAssertion EnableReason;

        /// <summary>
        /// 对象关闭条件
        /// </summary>
        public BaseAssertion DisableReason;

        /// <summary>
        /// 资源名字
        /// </summary>
        public string ResourceName;

        /// <summary>
        /// 资源类型
        /// </summary>
        public ObjectType Type;
    }

    /// <summary>
    /// 故事进度存档
    /// </summary>
    [Serializable]
    public struct ProgressSave
    {
        /// <summary>
        /// 消息通道
        /// </summary>
        public Dictionary<string, EventCenter.EventArgs> MessagePath;

        /// <summary>
        /// 已触发事件
        /// </summary>
        public List<string> HaveTriggered;
    }

    /// <summary>
    /// 仓库存档
    /// </summary>
    [Serializable]
    public struct InventorySave
    {
        /// <summary>
        /// Key:物品名<para/>
        /// Value:物品数量
        /// </summary>
        public Dictionary<string, int> Items;

        public InventorySave(InventorySave save)
        {
            Items = new Dictionary<string, int>(save.Items);
        }
    }

    /// <summary>
    /// 声音设置存档
    /// </summary>
    [Serializable]
    public struct SoundSave
    {
        public float Music;
        public float Effect;
    }

    /// <summary>
    /// 游戏设置存档
    /// </summary>
    public struct SettingSave
    {
        public SoundSave SoundSave;
        public Dictionary<KeyType, KeyCode> ControlSave;
    }

}

