using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameEndPopup : MonoBehaviour
{
    public static GameEndPopup Instance;
    public GameObject gameWinPanel;
    public GameObject gameLostPanel;

    private void Awake()
    {
        Instance = this;
        gameWinPanel.SetActive(false);
        gameLostPanel.SetActive(false);
    }

    public void WinningPopup()
    {
        gameWinPanel.SetActive(true);
    }

    public void LoosingPopup()
    {
        gameLostPanel.SetActive(true);
    }

    public void BackToMenuButton()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGameButton()
    {
        Application.Quit();
    }
}
