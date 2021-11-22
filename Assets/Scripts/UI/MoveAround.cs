using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MoveAround : MonoBehaviour
{
    public Transform Target;

    private Vector2 _oriPos;

    public float Velocity;

    private void Awake()
    {
        _oriPos = transform.position;
    }

    public void Move()
    {
        transform.DOMove(Target.position, Velocity).SetSpeedBased();
    }

    public void Back()
    {
        transform.DOMove(_oriPos, Velocity).SetSpeedBased();
    }
}
