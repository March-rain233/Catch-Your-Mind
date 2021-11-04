using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBaseModel
{
    /// <summary>
    /// 用于通知视图层数据更新
    /// </summary>
    /// <remarks>
    /// 需要外界知晓更新的数据，便在数据更新时调用该事件并传入变量名和值
    /// </remarks>
    event System.Action<string, object> NotifyEvent;
}
