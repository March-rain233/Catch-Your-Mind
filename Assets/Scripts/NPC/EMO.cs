using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EMO : MonoBehaviour
{
    public Transform Follow;
    public float Distance;

    //���ղ�������
    private Tweener tweener;

    void Start()
    {
        tweener = transform.DOMove(Follow.position, 2.0f).SetAutoKill(false).SetSpeedBased();
    }

    // Update is called once per frame
    void Update()
    {
        var dir = Follow.position - transform.position;
        //����Ŀ��㣬ChangeEndValue�ڶ�������snapStartValue������Ϊtrue���������ָ�����ʼλ�ã�snapStartValue�𵽴ӵ�ǰλ�ÿ�ʼ�����ã�Restartִ��
        if (dir.magnitude > Distance)
        {
            tweener.ChangeEndValue(transform.position + dir.normalized * (dir.magnitude - Distance), true).Restart();
        }
    }
}
