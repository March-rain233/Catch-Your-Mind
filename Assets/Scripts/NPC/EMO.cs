using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EMO : MonoBehaviour
{
    public Transform Follow;
    public Animator Animator;
    public float Distance;
    public float OffsetY;

    //接收操作动画
    private Tweener tweener;
    private Vector2 _lastPosition;

    void Start()
    {
        tweener = transform.DOMove(Follow.position, 2.0f).SetAutoKill(false).SetSpeedBased();
    }

    // Update is called once per frame
    void Update()
    {
        var follow = Follow.position;
        follow.y += OffsetY;

        var dir = follow - transform.position;
        float degree = 0.01f;
        float pos = transform.position.x - _lastPosition.x;
        if (pos < -degree)
        {
            Animator.SetFloat("FaceDirection", -1);
        }
        else if (pos > degree)
        {
            Animator.SetFloat("FaceDirection", 1);
        }
        _lastPosition = transform.position;

        if (dir.magnitude > Distance)
        {
            tweener.ChangeEndValue(transform.position + dir.normalized * (dir.magnitude - Distance), true).Restart();
        }
    }
}
