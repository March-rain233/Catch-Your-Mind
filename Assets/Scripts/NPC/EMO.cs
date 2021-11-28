using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EMO : MonoBehaviour
{
    public Transform Follow;
    public float Distance;

    //接收操作动画
    private Tweener tweener;

    void Start()
    {
        tweener = transform.DOMove(Follow.position, 2.0f).SetAutoKill(false).SetSpeedBased();
    }

    // Update is called once per frame
    void Update()
    {
        var dir = Follow.position - transform.position;
        //跟随目标点，ChangeEndValue第二个参数snapStartValue不设置为true跟随物体会恢复到初始位置，snapStartValue起到从当前位置开始的作用，Restart执行
        if (dir.magnitude > Distance)
        {
            tweener.ChangeEndValue(transform.position + dir.normalized * (dir.magnitude - Distance), true).Restart();
        }
    }
}
