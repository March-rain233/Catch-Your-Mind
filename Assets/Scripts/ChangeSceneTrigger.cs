using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneTrigger : MonoBehaviour
{
    public string Show;

    public string NextScene;

    public bool Stay;

    private void Update()
    {
        if (Stay)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                SceneManager.LoadScene(NextScene);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Stay = true;
            GameManager.Instance.EventCenter.SendEvent("InteractionEnter", new EventCenter.EventArgs() { String = Show });
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Stay = false;
            GameManager.Instance.EventCenter.SendEvent("InteractionExit", new EventCenter.EventArgs() { String = Show });
        }
    }
}
