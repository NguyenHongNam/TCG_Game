using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverPopup : MonoBehaviour
{
    public static GameOverPopup Instance;

    public GameObject gameOverPanel;
    public Text gameOverText;
    public AudioSource gameOverMusic;

    private void Awake()
    {
        Instance = this;
        gameOverPanel.SetActive(false);

        if (gameOverMusic == null)
        {
            gameOverMusic = GetComponent<AudioSource>();
        }
    }

    public void Show()
    {
        gameOverPanel.SetActive(true);
        if (gameOverMusic != null)
        {
            gameOverMusic.Play();
        }
    }

    public void Hide()
    {
        gameOverPanel.SetActive(false);
        if (gameOverMusic != null)
        {
            gameOverMusic.Stop();
        }
    }
    public void OnMainMenuButtonClicked()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
