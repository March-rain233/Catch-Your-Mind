using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PausePanel : MonoBehaviour
{
    public Button ResumeButton;
    public Button BackToMainMenuButton;
    public Button QuitButton;
    public CanvasGroup CanvasGroup;
    public bool IsShow;
    public bool IsPause;

    private void Awake()
    {
        ResumeButton.onClick.AddListener(ResumeGame);
        BackToMainMenuButton.onClick.AddListener(BackToMainMenu);
        QuitButton.onClick.AddListener(Quit);
        IsShow = IsPause = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(GameManager.Instance.ControlManager.KeyDic[KeyType.Esc]))
        {
            if (IsShow && IsPause)
            {
                ResumeGame();
            }
            if(!IsShow && !IsPause)
            {
                PauseGame();
            }
        }
    }

    private void PauseGame()
    {
        GameManager.Instance.Passing = false;
        ShowPanel();
    }

    public void ShowPanel()
    {
        float duration = 0.5f;
        CanvasGroup.DOFade(1, duration).onComplete = ()=>IsShow=true;
        CanvasGroup.blocksRaycasts = true;
        CanvasGroup.interactable = true;

        IsPause = true;
    }

    public void ClosePanel()
    {
        float duration = 0.5f;
        CanvasGroup.DOFade(0, duration).onComplete = ()=>IsShow=false;
        CanvasGroup.blocksRaycasts = false;
        CanvasGroup.interactable = false;

        IsPause = false;
    }

    private void ResumeGame()
    {
        //GameManager.Instance.Status = GameStatus.Pause;
        GameManager.Instance.Passing = true;
        ClosePanel();
    }
    private void BackToMainMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }
    private void Quit()
    {
        Application.Quit();
    }
}
