using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.Serialization;
using Sirenix.OdinInspector;

public class AnimatorSwitch : SerializedMonoBehaviour
{
    [OdinSerialize]
    public Animator Animator
    {
        get => _animator;
        set => _animator = value;
    }
    private Animator _animator;

    private void Awake()
    {
        if (!Animator) { Animator = GetComponent<Animator>(); }
    }

    public void Open()
    {
        Animator.enabled = true;
    }

    public void Close()
    {
        Animator.enabled = false;
    }
}
