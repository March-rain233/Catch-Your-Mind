using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaticDialog : Dialog
{
    public enum Person
    {
        None,
        Mind,
        Emo,
    }

    [SerializeField]
    private Image _headA;

    [SerializeField]
    private Image _headB;

    public Person HeadIcon
    {
        get => _person;
        set
        {
            _person = value;
            switch (_person)
            {
                case Person.None:
                    _headA.gameObject.SetActive(false);
                    _headB.gameObject.SetActive(false);
                    break;
                case Person.Mind:
                    _headA.gameObject.SetActive(true);
                    _headB.gameObject.SetActive(false);
                    break;
                case Person.Emo:
                    _headA.gameObject.SetActive(false);
                    _headB.gameObject.SetActive(true);
                    break;
            }
        }
    }
    private Person _person;
}
