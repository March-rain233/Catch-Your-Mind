using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Button GameStart;
    public Button Load;
    public Button Quit;

    private void Start()
    {
        GameStart.onClick.AddListener(StartGame);
        Quit.onClick.AddListener(QuitGame);
        Load.onClick.AddListener(LoadGame);

        if(GameManager.Instance.GameSave.SceneIndex == -1)
        {
            Load.interactable = false;
        }
    }

    private void StartGame()
    {
        GameManager.Instance.GameStart();
    }

    private void LoadGame()
    {
        GameManager.Instance.LoadGame();
    }

    private void QuitGame()
    {
        GameManager.Instance.Quit();
    }
}
