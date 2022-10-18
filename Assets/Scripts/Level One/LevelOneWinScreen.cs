using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelOneWinScreen : MonoBehaviour
{
    public GameObject pauseMenuObject;

    private PauseMenuControl pauseMenuControl;

    void Awake()
    {
        pauseMenuControl = pauseMenuObject.GetComponent<PauseMenuControl>();
    }

    public void LoadNextLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void RetryGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void MainMenuTransferXD()
    {
        Time.timeScale = 1f;
        pauseMenuControl.MainMenuTransfer();
    }

    public void QuitGameXD()
    {
        pauseMenuControl.QuitGame();
    }
}
