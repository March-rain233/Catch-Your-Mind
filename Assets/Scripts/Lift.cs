using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Lift : MonoBehaviour
{
    public Transform Target;
    public float Velocity;

    public bool Stay;
    public bool Running = false;
    private Vector2 Ori;
    bool isOri = true;
    public bool IsStory = false;

    private void Start()
    {
        Ori = transform.position;
        isOri = true;
        if (IsStory)
        {
            float timeCount = 0;
            DOTween.To(() => timeCount, a => timeCount = a, 1, 1f).OnComplete(() => GameManager.Instance.EventCenter.SendEvent("电梯上大心情", new EventCenter.EventArgs() { Boolean = true }));

            rise();
        }
    }

    private void Update()
    {
        if (Stay && !Running)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                rise();
            }
        }
    }

    public void rise()
    {
        Running = true;
        float y = isOri ? Target.position.y : Ori.y;
        isOri = !isOri;
        var mind = GameObject.Find("Mind").transform;
        mind.GetComponent<BoxCollider2D>().isTrigger = true;
        mind.GetComponent<Rigidbody2D>().isKinematic = true;
        var emo = GameObject.Find("EMO").transform;
        //GameManager.Instance.EventCenter.SendEvent("DIALOG_ENTER", new EventCenter.EventArgs());
        mind.GetComponent<NPC.BehaviorTreeRunner>().enabled = false;
        var tween = transform.DOMoveY(y, Velocity).SetSpeedBased();
        emo.DOMoveY(y, Velocity).SetSpeedBased();
        mind.DOMoveY(y, Velocity).SetSpeedBased().onComplete = () =>
        {
            //GameManager.Instance.EventCenter.SendEvent("DIALOG_EXIT", new EventCenter.EventArgs());
            mind.GetComponent<NPC.BehaviorTreeRunner>().enabled = true;
            mind.GetComponent<BoxCollider2D>().isTrigger = false;
            mind.GetComponent<Rigidbody2D>().isKinematic = false;
            Vector2 position = mind.position;
            position.y = y;
            mind.position = position;
            Running = false;
        };
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Stay = true;
            GameManager.Instance.EventCenter.SendEvent("InteractionEnter", new EventCenter.EventArgs() { String = "按E乘坐电梯" });
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Stay = false;
            GameManager.Instance.EventCenter.SendEvent("InteractionExit", new EventCenter.EventArgs() { String = "按E出门" });
        }
    }
}
